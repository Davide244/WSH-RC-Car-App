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
            var access = bleManager.RequestAccess();
            if (access != AccessState.Available)
            {
                // Do something
            }
        }
    }
}
