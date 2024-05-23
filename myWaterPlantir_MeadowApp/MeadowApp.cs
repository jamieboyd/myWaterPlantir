using Meadow;
using Meadow.Devices;
using Meadow.Hardware;
using Meadow.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using WaterPlantir.Controllers;
using WaterPlantir.MeadowApp.Hardware;
using WaterPlantir_MeadowApp.Bluetooth;

namespace WaterPlantir.MeadowApp
{
    public class MeadowApp : App<F7CoreComputeV2>
    {
        MainAppController mainAppController; // instance of class that controls the hardware

        public override Task Initialize()
        {

            // create our project specific hardware
            ProjectLabHardware hardware = new ProjectLabHardware();
            Resolver.Log.Info("Hardware up.");

            mainAppController = new MainAppController(hardware);
            Resolver.Log.Info("MainAppController up.");

            //====  wire up the wifi events
            WireUpWiFiStatusEvents();

            //==== setup our cloud logger
            var cloudLogger = new CloudLogger(LogLevel.Warning);
            Resolver.Log.AddProvider(cloudLogger);
            Resolver.Services.Add(cloudLogger);
            Resolver.Log.Info($"{(cloudLogger is null ? "Cloud Logger is null." : "Cloud Logger initialized.")}");

            // ====BlueTooth Server
            var bluetoothServer = new BluetoothServer(mainAppController);
            Resolver.Log.Info("BluetoothServer up.");


            return base.Initialize();
        }

        /// <summary>
        /// Called after Initalize returns.
        /// </summary>
        public override Task Run()
        {
            Resolver.Log.Info("Run...");

            mainAppController.Run();

            return base.Run();
        }

        /// <summary>
        /// Wires up the WiFi connection/disconnection events so the WiFi
        /// connection status is displayed to the user.
        /// </summary>
        ///

        void WireUpWiFiStatusEvents()
        {
            // get the wifi adapter
            var wifiAdapter = Resolver.Device.NetworkAdapters.Primary<IWiFiNetworkAdapter>();

            if (wifiAdapter is { } wifi)
            {
                // set initial state
                if (wifi.IsConnected)
                {
                    mainAppController?.SetWiFiStatus(true);
                    Resolver.Log.Info("Already connected to WiFi.");
                }
                else
                {
                    mainAppController?.SetWiFiStatus(false);
                    Resolver.Log.Info("Not connected to WiFi yet.");
                }
                // connect event
                wifi.NetworkConnected += (networkAdapter, networkConnectionEventArgs) =>
                {
                    Resolver.Log.Info($"Joined network - IP Address: {networkAdapter.IpAddress}");
                    mainAppController?.SetWiFiStatus(true);
                };
                // disconnect event
                // wifi.NetworkDisconnected += sender => { mainAppController?.SetWiFiStatus(false); };
            }
            else
            {
                Resolver.Log.Info("WiFi Adapter could not be found.");
            }
        }


        /// <summary>
        /// Configures the watchdog (automatic hardware reset on failure), and
        /// starts petting it.
        /// </summary>
        void WireUpWatchdogs()
        {
            var watchdogTimeout = TimeSpan.FromSeconds(30);
            var pettingInterval = TimeSpan.FromSeconds(20); // should be well less than the timeout

            // Enable the watchdog for 30 second intervals (max is ~32s)
            Device.WatchdogEnable(watchdogTimeout);
            // Start the thread that resets the counter.
            StartPettingWatchdog(pettingInterval);
        }

        /// <summary>
        /// Pets the watchdog which tells the hardware not to restart. As long
        /// as the app is running, this should also run and keep the board up.
        /// </summary>
        /// <param name="pettingInterval">How often to pet the watchdog.</param>
        void StartPettingWatchdog(TimeSpan pettingInterval)
        {
            // Just for good measure, let's reset the watchdog to begin with.
            Device.WatchdogReset();
            // Start a thread that restarts it.
            Thread t = new Thread(async () =>
            {
                while (true)
                {
                    Thread.Sleep(pettingInterval);
                    Device.WatchdogReset();
                }
            });
            t.Start();
        }
    }
}
