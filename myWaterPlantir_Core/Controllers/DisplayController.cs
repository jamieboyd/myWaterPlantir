using Meadow;
using Meadow.Foundation.Graphics;
using Meadow.Foundation.Graphics.MicroLayout;
using Meadow.Peripherals.Displays;
using WaterPlantir.Models;

namespace WaterPlantir.Controllers
{
    public class DisplayController
    {
        private readonly DisplayScreen screen;

        private Font12x20 font12X20 = new Font12x20();
        private Font12x16 font12x16 = new Font12x16();

        protected Label StatusLabel { get; set; }

        protected Label DistanceLabel { get; set; }

        protected Label Temp1Label { get; set; }

        protected Label O2Label { get; set; }

        protected Picture WiFi { get; set; }

        protected Picture Sync { get; set; }

        public DisplayController(IPixelDisplay _display, RotationType rotation)
        {
            screen = new DisplayScreen(_display, rotation);

            screen.Controls.Add(new Box(0, 0, screen.Width, screen.Height)
            {
                ForeColor = Color.White
            });

            screen.Controls.Add(new Box(0, 30, 106, 95)
            {
                ForeColor = Color.FromHex("#B35E2C")
            });
            screen.Controls.Add(new Box(107, 30, 106, 95)
            {
                ForeColor = Color.FromHex("#1A80AA")
            });
            screen.Controls.Add(new Box(214, 30, 106, 95)
            {
                ForeColor = Color.FromHex("#98A645")
            });

            screen.Controls.Add(new Box(160, 125, 1, 115)
            {
                ForeColor = Color.Black,
                IsFilled = false
            });
            screen.Controls.Add(new Box(0, 180, screen.Width, 1)
            {
                ForeColor = Color.Black,
                IsFilled = false
            });

            StatusLabel = new Label(5, 5, 12, 16)
            {
                Text = "Initializing 1,2,3..",
                Font = font12X20,
                TextColor = Color.Black
            };
            screen.Controls.Add(StatusLabel);


            screen.Controls.Add(new Label(5, 35, 96, 16)
            {
                Text = "From Top",
                Font = font12x16,
                TextColor = Color.White
            });
            screen.Controls.Add(new Label(5, 100, 96, 20)
            {
                Text = "cm",
                Font = font12X20,
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Right
            });

            screen.Controls.Add(new Label(111, 35, 96, 16)
            {
                Text = "Temp",
                Font = font12x16,
                TextColor = Color.White
            });
            screen.Controls.Add(new Label(112, 100, 96, 20)
            {
                Text = "°C",
                Font = font12X20,
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Right
            });

            screen.Controls.Add(new Label(219, 35, 96, 16)
            {
                Text = "O2 Conc",
                Font = font12x16,
                TextColor = Color.White,
            });
            screen.Controls.Add(new Label(219, 100, 96, 20)
            {
                Text = "mg/L",
                Font = font12X20,
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Right,
            });

            DistanceLabel = new Label(5, 63, 96, 32, ScaleFactor.X2)
            {
                Text = "0",
                Font = font12x16,
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            screen.Controls.Add(DistanceLabel);
            Temp1Label = new Label(112, 63, 96, 32, ScaleFactor.X2)
            {
                Text = "0",
                Font = font12x16,
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            screen.Controls.Add(Temp1Label);
            O2Label = new Label(219, 63, 96, 32, ScaleFactor.X2)
            {
                Text = "0",
                Font = font12x16,
                TextColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };
            screen.Controls.Add(O2Label);
        }

        public void UpdateStatus(string status)
        {
            StatusLabel.Text = status;
        }

        public void UpdateModel(WaterPlantirConditionsModel model)
        {
            screen.BeginUpdate();

            DistanceLabel.Text = model.DistanceToTopOfLiquid?.Centimeters.ToString("N1") ?? "n/a";
            Temp1Label.Text = model.ThermistorOneTemp?.Celsius.ToString("N1") ?? "n/a";
            O2Label.Text = model.Concentration?.MilligramsPerLiter.ToString("N1") ?? "n/a";

            screen.EndUpdate();
        }
    }
}