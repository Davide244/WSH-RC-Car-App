using Shiny;
using Shiny.BluetoothLE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSHRCCarController.Services
{
    public struct RCBluetoothDevice
    {
        public string Name;
        public string Address;
        public string Status;
        public string Manufacturer;
    }

    public enum RCDataType
    {
        MotorStateChange
    }

    public struct RCData
    {
        public RCDataType Type;
        public int Speed;
        public int Steer;
    }

    public interface IBluetoothService
    {
        List<RCBluetoothDevice> GetAvailableDevices();

        void ConnectToDevice(RCBluetoothDevice device);
        void DisconnectDevice(RCBluetoothDevice device);

        bool isDeviceConnected(RCBluetoothDevice device);

        void SendData(RCData data);
    }
}
