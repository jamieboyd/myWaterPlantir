using Meadow.Units;

namespace WaterPlantir.Models
{
    public class WaterPlantirConditionsModel
    {
        public Temperature? Temperature { get; set; }
        public Pressure? Pressure { get; set; }
        public RelativeHumidity? Humidity { get; set; }
        public Temperature? ThermistorOneTemp { get; set; }
        public Length? DistanceToTopOfLiquid { get; set; }
        public ConcentrationInWater? Concentration { get; set; }
    }
}
