using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace WSHRCCarController;

public partial class BluetoothDeviceListItem : ObservableObject 
{
	[ObservableProperty]
	public string _deviceName;
	[ObservableProperty]
	public string _deviceAddress;
	[ObservableProperty]
	public string _deviceStatus;
	[ObservableProperty]
	public string _deviceManufacturer;

	public BluetoothDeviceListItem(string deviceName, string deviceAddress, string deviceStatus, string deviceManufacturer)
    {
        DeviceName = deviceName;
        DeviceAddress = deviceAddress;
        DeviceStatus = deviceStatus;
        DeviceManufacturer = deviceManufacturer;
    }
}

public partial class BluetoothDeviceItemViewmodel : ObservableObject
{
	[ObservableProperty]
	public ObservableCollection<BluetoothDeviceListItem> _connectedBluetoothDeviceList = new();

	[ObservableProperty]
	public ObservableCollection<BluetoothDeviceListItem> _availableBluetoothDeviceList = new();
}

public partial class BluetoothPage : ContentPage
{
	BluetoothDeviceItemViewmodel bluetoothDeviceItemViewmodel = new();
	public BluetoothPage()
	{


        InitializeComponent();

#if ANDROID
        // Current Activity as current MainActivity
        WSHRCCarController.MainActivity currentActivity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity as WSHRCCarController.MainActivity;
        currentActivity.SetOrientation(WSHRCCarController.DeviceOrientation.Portrait);
#endif

        ConnectedBluetoothDeviceList.ItemsSource = bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList;
        ConnectedBluetoothDeviceList.SelectionMode = ListViewSelectionMode.None;

		AvailableBluetoothDeviceList.ItemsSource = bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList;
		AvailableBluetoothDeviceList.SelectionMode = ListViewSelectionMode.None;

		// Add some dummy data
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 1", "00:00:00:00:00:00", "Connected", "Manufacturer 1"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 2", "00:00:00:00:00:01", "Disconnected", "Manufacturer 2"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 3", "00:00:00:00:00:02", "Connected", "Manufacturer 3"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 4", "00:00:00:00:00:03", "Disconnected", "Manufacturer 4"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 5", "00:00:00:00:00:04", "Connected", "Manufacturer 5"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 6", "00:00:00:00:00:05", "Disconnected", "Manufacturer 6"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 7", "00:00:00:00:00:06", "Connected", "Manufacturer 7"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 8", "00:00:00:00:00:07", "Disconnected", "Manufacturer 8"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 9", "00:00:00:00:00:08", "Connected", "Manufacturer 9"));
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 10", "00:00:00:00:00:09", "Disconnected", "Manufacturer 10"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 1", "00:00:00:00:00:00", "Connected", "Manufacturer 1"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 2", "00:00:00:00:00:01", "Disconnected", "Manufacturer 2"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 3", "00:00:00:00:00:02", "Connected", "Manufacturer 3"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 4", "00:00:00:00:00:03", "Disconnected", "Manufacturer 4"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 5", "00:00:00:00:00:04", "Connected", "Manufacturer 5"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 6", "00:00:00:00:00:05", "Disconnected", "Manufacturer 6"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 7", "00:00:00:00:00:06", "Connected", "Manufacturer 7"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 8", "00:00:00:00:00:07", "Disconnected", "Manufacturer 8"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 9", "00:00:00:00:00:08", "Connected", "Manufacturer 9"));
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 10", "00:00:00:00:00:09", "Disconnected", "Manufacturer 10"));

		bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 11", "00:00:00:00:00:10", "Connected", "Manufacturer 11"));
		bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 12", "00:00:00:00:00:11", "Disconnected", "Manufacturer 12"));
		bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Add(new BluetoothDeviceListItem("Device 13", "00:00:00:00:00:12", "Connected", "Manufacturer 13"));
    }

    private void AvailableBluetoothDeviceList_ChildrenChanged(object sender, ElementEventArgs e)
    {
		if (bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Count <= 0)
			BluetoothNoAvailableDevicesIndicator.IsVisible = true;
		else
			BluetoothNoAvailableDevicesIndicator.IsVisible = false;
    }

    private void ConnectedBluetoothDeviceList_ChildrenChanged(object sender, ElementEventArgs e)
    {

    }
}