using MauiIcons.Core;
using System.Diagnostics;

namespace WSHRCCarController
{
    public partial class MainPage : ContentPage
    {

        private int MotorSpeed = 0;         // NOTE: This is from 0 to 255. 0 is no speed, 255 is full speed.
        private int MotorSteerAngle = 0; // NOTE: This is not in degrees, but in a range of -100 to 100. -100 is full left, 100 is full right.

        private const float MotorSteerSensitivity = 1.2f;

        public MainPage()
        {
            InitializeComponent();
            _ = new MauiIcon();
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            Services.IBluetoothService bluetoothService = DependencyService.Get<Services.IBluetoothService>();
            bluetoothService.SendData(new Services.RCData { Type = Services.RCDataType.Speed, value = 100 });
        }

        private void JoyXPanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            // multiply by MotorSpeedMultiplier to adjust the slider sensitivity
            float actualMotorAngle = (float)(e.TotalX * MotorSteerSensitivity);
            int rawTranslationX = (int)e.TotalX;

            // This moves JoystickX_Stick to the cursor position
            if (actualMotorAngle > 255 || actualMotorAngle < -255)
            {
                actualMotorAngle = Math.Clamp(actualMotorAngle, -255, 255);
                rawTranslationX = (int)(actualMotorAngle / MotorSteerSensitivity);
            }

            JoystickX_Stick.TranslationX = rawTranslationX;
            // Set Motor steer angle (Round value correctly)
            MotorSteerAngle = (int)Math.Round(actualMotorAngle);

            Debug.Print($"MotorSteerAngle: {MotorSteerAngle}; RAW: {e.TotalX}");

            // Send the data to the car
            Services.IBluetoothService bluetoothService = DependencyService.Get<Services.IBluetoothService>();
            bluetoothService.SendData(new Services.RCData { Type = Services.RCDataType.Steer, value = MotorSteerAngle });
        }
    }

}
