using Meadow;
using Meadow.Devices;
using Meadow.Peripherals.Sensors;

using Meadow.Foundation.Sensors.Environmental;
using Meadow.Foundation.Sensors.Temperature;
using Meadow.Foundation.Sensors.Distance;
using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;

using Meadow.Peripherals.Sensors.Environmental;
using Meadow.Foundation.Grove.Relays;
using Meadow.Units;
using System;
using WaterPlantir.Hardware;
using Meadow.Peripherals.Sensors.Distance;
using TempCorrectedDOSensorContract;

namespace WaterPlantir.MeadowApp.Hardware
{
    public class ProjectLabHardware : IWaterPlantirHardware
    {
        //==== Project Lab bits, just the bare minimum here
        protected IProjectLabHardware projectLab { get; set; }
        public IPixelDisplay Display => projectLab.Display;

        //==== external peripherals - relays
        // 2 4 channel relays (we use SPDT Relays with I2C control)
        public FourChannelSpdtRelay RelayModule_12V { get; set; } = default!;
        public FourChannelSpdtRelay RelayModule_110V { get; set; } = default!;
        // functional aliases we will make for each channel
        public IRelay Relay110_Sump { get; set; } = default!;
        public IRelay Relay110_Aerator { get; set; } = default!;
        public IRelay Relay110_Heater { get; set; } = default!;
        public IRelay Relay110_Extra { get; set; } = default!;
        public IRelay Relay12_Bilge1 { get; set; } = default!;
        public IRelay Relay12_Bilge2 { get; set; } = default!;
        public IRelay Relay12_Stir { get; set; } = default!;
        public IRelay Relay12_Extra { get; set; } = default!;

        //==== external peripherals - distance sensor
        public IRangeFinder DistanceSensor { get; set; } = default!;

        //==== external peripherals - temperature corrected dissolved oxygen sensor
        // includes an attached ITemperatureSensor
        public ITemperatureSensor Thermistor_One { get; set; } = default!;
        public ITempCorrectedDOsensor DissolvedOxygenMeter { get; set; } = default!;

        public ProjectLabHardware()
        {
            //---- instantiate the project lab hardware
            projectLab = ProjectLab.Create();
            Resolver.Log.Info($"Running on ProjectLab Hardware {projectLab.RevisionString}");            

            //---- relay board for 110V stuff, using address 0x12 instead of default address 0x11
            Resolver.Log.Info("Loading 110V relay board...");
            try
            {
                RelayModule_110V = new FourChannelSpdtRelay(projectLab.Qwiic.I2cBus, 0x12);
                // create shortcuts to the relays
                Relay110_Sump = RelayModule_110V.Relays[0];
                Relay110_Aerator = RelayModule_110V.Relays[1];
                Relay110_Heater = RelayModule_110V.Relays[2];
                Relay110_Extra = RelayModule_110V.Relays[3];
                Resolver.Log.Info($"instantiated 110V relay with addr 0x12.");
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Could not instantiate 110V relay with 0x12: {ex.Message}");
                try
                {
                    RelayModule_110V = new FourChannelSpdtRelay(projectLab.Qwiic.I2cBus, 0x11);
                    RelayModule_110V.SetI2cAddress(0x12);
                    // create shortcuts to the relays
                    Relay110_Sump = RelayModule_110V.Relays[0];
                    Relay110_Aerator = RelayModule_110V.Relays[1];
                    Relay110_Heater = RelayModule_110V.Relays[2];
                    Relay110_Extra = RelayModule_110V.Relays[3];
                    Resolver.Log.Info($"instantiated 110V relay and switched addr to 0x12.");

                }
                catch (Exception ex1)
                {
                    Resolver.Log.Error($"Could not instantiate 110V relay with addr 0x11 : {ex1.Message}");
                }

            }
            //---- relay board for 12V stuff, using address 0x13
            Resolver.Log.Info("Loading 12V relay board...");
            try
            {
                RelayModule_12V = new FourChannelSpdtRelay(projectLab.Qwiic.I2cBus, 0x13);
                // create shortcuts to the relays
                Relay12_Bilge1 = RelayModule_12V.Relays[0];
                Relay12_Bilge2 = RelayModule_12V.Relays[1];
                Relay12_Stir = RelayModule_12V.Relays[2];
                Relay12_Extra = RelayModule_12V.Relays[3];
                Resolver.Log.Info($"instantiated 12V relay at addr 0x13.");
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Could not instantiate 12V relay with 0x13: {ex.Message}");
                try
                {
                    RelayModule_12V = new FourChannelSpdtRelay(projectLab.Qwiic.I2cBus, 0x11);
                    RelayModule_110V.SetI2cAddress(0x13);

                    // create shortcuts to the relays
                    Relay12_Bilge1 = RelayModule_12V.Relays[0];
                    Relay12_Bilge2 = RelayModule_12V.Relays[1];
                    Relay12_Stir = RelayModule_12V.Relays[2];
                    Relay12_Extra = RelayModule_12V.Relays[3];
                    Resolver.Log.Info($"instantiated 12V relay and switched addr to 0x13.");
                }
                catch
                {
                    Resolver.Log.Error($"Could not instantiate 12V relay with 0x11: {ex.Message}");
                }
            }


            //---- distance sensor
            Resolver.Log.Info($"Creating distance sensor.");
            try
            {
                DistanceSensor = new A02yyuw(projectLab.GroveUart.CreateSerialPort(), A02yyuw.MODE_UART_CONTROL);
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Could not instantiate distance sensor: {ex.Message}");
            }

            //---- dissolved oxygen sensor with attaches thermistor for temperature sensor
            Resolver.Log.Info($"Creating dissolved oxygen sensor.");
            try
            {
                //projectLab.MikroBus2.Pins.AN
                //IAnalogInputPort analogInputPort = projectLab.IOTerminal.Pins.A1.CreateAnalogInputPort(10, TimeSpan.FromMilliseconds(40), new Voltage(3.3));
                //DissolvedOxygenMeter = new DFRobotGravityDOMeter(projectLab.IOTerminal.Pins.A1.CreateAnalogInputPort(20), Thermistor_One);

                Thermistor_One = new SteinhartHartCalculatedThermistor(projectLab.GroveAnalog.Pins.D0.CreateAnalogInputPort(10),
                    new Resistance(10, Resistance.UnitType.Kiloohms));
                DissolvedOxygenMeter = new DFRobotGravityDOMeter.DFRobotGravityDOMeter (projectLab.IOTerminal.Pins.A1.CreateAnalogInputPort(20), Thermistor_One);
            }
            catch (Exception ex)
            {
                Resolver.Log.Error($"Could not instantiate dissolved oxygen sensor: {ex.Message}");


                Resolver.Log.Info($"Hardware initialization complete.");
            }
        }
    }
}