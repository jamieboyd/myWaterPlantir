using Meadow;
using Meadow.Gateways.Bluetooth;
using WaterPlantir_MeadowApp.Bluetooth;
using WaterPlantir.MeadowApp;
using WaterPlantir.Controllers;
using WaterPlantir.Models;

namespace WaterPlantir_MeadowApp.Bluetooth
{
    public class BluetoothServer
    {
        public static BluetoothServer Current { get; protected set; }

        Definition bleTreeDefinition;
        /*
        ICharacteristic temperatureCharacteristic;
        ICharacteristic humitidyCharacteristic;
        ICharacteristic pressureCharacteristic;
        */
        ICharacteristic therm1Characteristic;
       // ICharacteristic therm2Characteristic;
        ICharacteristic distToTopCharacteristic;
        ICharacteristic O2concCharacteristic;

        ICharacteristic relay110SumpCharacteristic;

        MainAppController mainAppController;

        public BluetoothServer(MainAppController appController)
        {
            bleTreeDefinition = GetDefinition();

            mainAppController = appController;
            mainAppController.ConditionsUpdated += MainAppControllerConditionsUpdated;

            MeadowApp.Device.BluetoothAdapter.StartBluetoothServer(bleTreeDefinition);



        }

        private void MainAppControllerConditionsUpdated(object sender, WaterPlantirConditionsModel e)
        {
           // temperatureCharacteristic.SetValue(e.Temperature.Value.Celsius.ToString());
            //humitidyCharacteristic.SetValue(e.Humidity.Value.Percent.ToString());
            //pressureCharacteristic.SetValue(e.Pressure.Value.StandardAtmosphere.ToString());
            therm1Characteristic.SetValue(e.ThermistorOneTemp.Value.Celsius.ToString());
            //therm2Characteristic.SetValue(e.ThermistorTwoTemp.Value.Celsius.ToString());
            distToTopCharacteristic.SetValue(e.DistanceToTopOfLiquid.Value.Centimeters.ToString());
            O2concCharacteristic.SetValue(e.Concentration.Value.MilligramsPerLiter.ToString());
        }

        Definition GetDefinition()
        {
            /*temperatureCharacteristic = new CharacteristicString(
               name: nameof(BluetoothCharacteristics.TEMPERATURE),
               uuid: BluetoothCharacteristics.TEMPERATURE,
               maxLength: 20,
               permissions: CharacteristicPermission.Read,
               properties: CharacteristicProperty.Read);

           humitidyCharacteristic = new CharacteristicString(
               name: nameof(BluetoothCharacteristics.HUMIDITY),
               uuid: BluetoothCharacteristics.HUMIDITY,
               maxLength: 20,
               permissions: CharacteristicPermission.Read,
               properties: CharacteristicProperty.Read);

           pressureCharacteristic = new CharacteristicString(
               name: nameof(BluetoothCharacteristics.PRESSURE),
               uuid: BluetoothCharacteristics.PRESSURE,
               maxLength: 20,
               permissions: CharacteristicPermission.Read,
               properties: CharacteristicProperty.Read);

           */
            therm1Characteristic = new CharacteristicString(
               name: nameof(BluetoothCharacteristics.THERM1),
               uuid: BluetoothCharacteristics.THERM1,
               maxLength: 20,
               permissions: CharacteristicPermission.Read,
               properties: CharacteristicProperty.Read);

            /*
            therm2Characteristic = new CharacteristicString(
             name: nameof(BluetoothCharacteristics.THERM2),
             uuid: BluetoothCharacteristics.THERM2,
             maxLength: 20,
             permissions: CharacteristicPermission.Read,
             properties: CharacteristicProperty.Read);
            */
            distToTopCharacteristic = new CharacteristicString(
             name: nameof(BluetoothCharacteristics.DIST_TO_TOP),
             uuid: BluetoothCharacteristics.DIST_TO_TOP,
             maxLength: 20,
             permissions: CharacteristicPermission.Read,
             properties: CharacteristicProperty.Read);

            O2concCharacteristic = new CharacteristicString(
             name: nameof(BluetoothCharacteristics.O2CONC),
             uuid: BluetoothCharacteristics.O2CONC,
             maxLength: 20,
             permissions: CharacteristicPermission.Read,
             properties: CharacteristicProperty.Read);

            relay110SumpCharacteristic = new CharacteristicBool(
                name: nameof(BluetoothCharacteristics.RELAY110_SUMP),
                uuid: BluetoothCharacteristics.RELAY110_SUMP,
                permissions: CharacteristicPermission.Read | CharacteristicPermission.Write,
                properties: CharacteristicProperty.Read | CharacteristicProperty.Write);


            var service = new Service(
                name: "Service",
                uuid: 253,
                //temperatureCharacteristic,
                //humitidyCharacteristic,
                //pressureCharacteristic,
                therm1Characteristic,
                //therm2Characteristic,
                distToTopCharacteristic,
                O2concCharacteristic,
                relay110SumpCharacteristic
            );

            return new Definition("WaterPlantir", service);
        }
    }
}