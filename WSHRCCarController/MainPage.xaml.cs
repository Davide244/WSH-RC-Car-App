using MauiIcons.Core;

namespace WSHRCCarController
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            _ = new MauiIcon();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count--;
        }

        private void BluetoothButton_Clicked(object sender, EventArgs e)
        {
            // Navigate to the BluetoothPage
            Navigation.PushAsync(new BluetoothPage());
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
#if ANDROID
        // Current Activity as current MainActivity
        WSHRCCarController.MainActivity currentActivity = Microsoft.Maui.ApplicationModel.Platform.CurrentActivity as WSHRCCarController.MainActivity;
        currentActivity.SetOrientation(WSHRCCarController.DeviceOrientation.Landscape);
#endif
        }
    }

}
