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
    }

    private void AvailableBluetoothDeviceList_ChildrenChanged(object sender, ElementEventArgs e)
    {
		if (bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Count <= 0)
			BluetoothNoAvailableDevicesIndicator.IsVisible = false;
		else
			BluetoothNoAvailableDevicesIndicator.IsVisible = true;
    }

    private void ConnectedBluetoothDeviceList_ChildrenChanged(object sender, ElementEventArgs e)
    {

    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
		Services.IBluetoothService bluetoothService = DependencyService.Get<Services.IBluetoothService>();
		List<Services.RCBluetoothDevice> availableDevices = bluetoothService.GetAvailableDevices();

		bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Clear();
		foreach (Services.RCBluetoothDevice device in availableDevices)
		{
			bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Add(new BluetoothDeviceListItem(device.Name, device.Address, device.Status, device.Manufacturer));
		}
    }

    private void ConnectToDeviceBtn_Clicked(object sender, EventArgs e)
    {
		// get data context of sender
		BluetoothDeviceListItem device = (BluetoothDeviceListItem)((Button)sender).BindingContext;

		Services.IBluetoothService bluetoothService = DependencyService.Get<Services.IBluetoothService>();
		bluetoothService.ConnectToDevice(new Services.RCBluetoothDevice { Name = device.DeviceName, Address = device.DeviceAddress, Status = device.DeviceStatus, Manufacturer = device.DeviceManufacturer });

		// add device to connected devices
		bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Add(device);
		// Remove device from available devices
		bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Remove(device);
    }

    private void DisconnectFromDeviceBtn_Clicked(object sender, EventArgs e)
    {
        // get data context of sender
        BluetoothDeviceListItem device = (BluetoothDeviceListItem)((Button)sender).BindingContext;

        Services.IBluetoothService bluetoothService = DependencyService.Get<Services.IBluetoothService>();
        bluetoothService.DisconnectDevice(new Services.RCBluetoothDevice { Name = device.DeviceName, Address = device.DeviceAddress, Status = device.DeviceStatus, Manufacturer = device.DeviceManufacturer });

        // add device to connected devices
        bluetoothDeviceItemViewmodel.ConnectedBluetoothDeviceList.Remove(device);
        // Remove device from available devices
        bluetoothDeviceItemViewmodel.AvailableBluetoothDeviceList.Add(device);
    }
}