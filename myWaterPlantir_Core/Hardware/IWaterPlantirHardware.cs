using Meadow.Peripherals.Displays;
using Meadow.Peripherals.Relays;
using Meadow.Peripherals.Sensors;
using Meadow.Peripherals.Sensors.Distance;
using Meadow.Peripherals.Sensors.Environmental;
using TempCorrectedDOSensorContract;
namespace WaterPlantir.Hardware
{
    public interface IWaterPlantirHardware
    {
        //--- project lab bits
        IPixelDisplay? Display { get; }
        /*
        IRgbPwmLed? RgbLed { get; }
        ITemperatureSensor? TemperatureSensor { get; }
        IHumiditySensor? HumiditySensor { get; }
        IBarometricPressureSensor? PressureSensor { get; }
        IButton? LeftButton { get; }
        IButton? RightButton { get; }
        IButton? UpButton { get; }
        IButton? DownButton { get; }
        */
        //---- external peripherals
        ITemperatureSensor Thermistor_One { get; }
        //IDissolvedOxygenConcentrationSensor DissolvedOxygenMeter { get; }
        ITempCorrectedDOsensor DissolvedOxygenMeter  {get; }
         IRangeFinder DistanceSensor { get; }
        // ---- 2 4 -channel i2c Relays
        IRelay Relay110_Sump { get; set; }
        IRelay Relay110_Aerator { get; set; }
        IRelay Relay110_Heater { get; set; }
        IRelay Relay110_Extra { get; set; }
        IRelay Relay12_Bilge1 { get; set; }
        IRelay Relay12_Bilge2 { get; set; }
        IRelay Relay12_Stir { get; set; }
        IRelay Relay12_Extra { get; set; }
    }
}