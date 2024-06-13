using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Java.Util;
using Android.Bluetooth;
using WSHRCCarController.Services;
using WSHRCCarController.Platforms.Android.Bluetooth;
using Android.OS;

//[assembly: Dependency(typeof(BluetoothConnector))]
namespace WSHRCCarController.Platforms.Android.Bluetooth
{
    public class BluetoothConnector : IBluetoothService
    {
        private BluetoothAdapter _bluetoothAdapter;
        private BluetoothDevice _bluetoothDevice;
        private List<BluetoothSocket> bluetoothSockets = new();

        public BluetoothConnector()
        {
            _bluetoothAdapter = BluetoothAdapter.DefaultAdapter;
        }

        public List<RCBluetoothDevice> GetAvailableDevices()
        {
            List<RCBluetoothDevice> connectedDevices = new List<RCBluetoothDevice>();

            foreach (BluetoothDevice device in _bluetoothAdapter.BondedDevices)
            {
                connectedDevices.Add(new RCBluetoothDevice
                {
                    Name = device.Name,
                    Address = device.Address,
                    Status = "Connected",
                    Manufacturer = "Unknown"
                });
            }

            return connectedDevices;
        }

        public void ConnectToDevice(RCBluetoothDevice device)
        {
            _bluetoothDevice = _bluetoothAdapter.GetRemoteDevice(device.Address);

            //var method = Device.GetType().GetMethod("createRfcommSocket");

            //BluetoothSocket _bluetoothSocket = _bluetoothDevice.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            // Print UUIDs to debug
            BluetoothSocket _bluetoothSocket = _bluetoothDevice.CreateInsecureRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            //try
            //{
            //    _bluetoothSocket.Connect();
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //    return;
            //}

            _bluetoothSocket.Connect();

            bluetoothSockets.Add(_bluetoothSocket);
        }

        public void DisconnectDevice(RCBluetoothDevice device)
        {
            BluetoothSocket _bluetoothSocket = bluetoothSockets.Find(socket => socket.RemoteDevice.Address == device.Address);
            _bluetoothSocket.Close();
            bluetoothSockets.Remove(_bluetoothSocket);
        }

        public bool isDeviceConnected(RCBluetoothDevice device)
        {
            // Check if device has an open socket
            return bluetoothSockets.Contains(bluetoothSockets.Find(socket => socket.RemoteDevice.Address == device.Address));
        }

        public void SendData(RCData data)
        {
            // Convert Type to byte
            byte type = data.Type switch
            {
                RCDataType.Speed => 0x01,
                RCDataType.Steer => 0x02,
                _ => 0x00
            };

            // Convert int to byte array
            byte[] value = BitConverter.GetBytes((short)data.value);

            // Combine type and value
            byte[] dataToSend = new byte[3];
            dataToSend[0] = type;
            dataToSend[1] = value[0];
            dataToSend[2] = value[1];

            // Send data to all connected devices
            foreach (BluetoothSocket socket in bluetoothSockets)
            {
                socket.OutputStream.Write(dataToSend, 0, dataToSend.Length);
            }
        }
    }
}
