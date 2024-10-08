﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Java.Util;
using Android.Bluetooth;
using WSHRCCarController.Services;
using WSHRCCarController.Platforms.Android.Bluetooth;
using Android.OS;
using Java.Lang;

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

        const byte SignatureByte = 0xAA;
        const byte Signature2Byte = 0x01;
        const byte SignatureEndByte = 0x1C;

        public void SendData(RCData data)
        {
            // Convert to byte array. Structure: [Signature, Identifier, Data, Data2 (Direction for Motors)]
            byte[] dataBytes = new byte[8];
            byte IdentifierByte = (byte)data.Type;

            dataBytes[0] = SignatureByte;
            dataBytes[1] = Signature2Byte;

            if (data.Type == RCDataType.MotorStateChange) 
            {
                dataBytes[2] = IdentifierByte;
                dataBytes[3] = (byte)(System.Math.Abs(data.Speed));
                dataBytes[4] = (byte)(data.Speed < 0 ? 0xFF : 0x00);
                dataBytes[5] = (byte)(System.Math.Abs(data.Steer));
                dataBytes[6] = (byte)(data.Steer < 0 ? 0xFF : 0x00);
            }

            dataBytes[7] = SignatureEndByte;

            // Print to debug
            Console.WriteLine("Sending data: " + BitConverter.ToString(dataBytes));

            // Send data to all connected devices
            foreach (BluetoothSocket socket in bluetoothSockets)
            {
                socket.OutputStream.Write(dataBytes, 0, dataBytes.Length); 
            }
        }
    }
}
