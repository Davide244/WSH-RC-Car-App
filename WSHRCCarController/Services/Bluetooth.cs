using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSHRCCarController.Services
{
    public class BluetoothService
    {
        IBleManager bleManager;

        BluetoothService()
        {
            // Set up the BLE manager
            bleManager = Shiny.Hosting.Host.GetService<IBleManager>();

            var access = bleManager.RequestAccess();
            if (access.Equals(AccessState.Available))
            {
                // Do something
            }
        }

        void ScanForDevices()
        {
            // Scan for devices
            var scan = bleManager.ScanForUniquePeripherals();
            scan.Subscribe(peripheral =>
            {
                // Do something
            });
        }
    }
}
