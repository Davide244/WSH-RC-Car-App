using MauiIcons.Core;
using System.Diagnostics;

namespace WSHRCCarController
{
    public partial class MainPage : ContentPage
    {

        private int MotorSpeed = 0;         // NOTE: This is from 0 to 255. 0 is no speed, 255 is full speed.
        private int MotorSteerAngle = 0; // NOTE: This is not in degrees, but in a range of -100 to 100. -100 is full left, 100 is full right.

        private const float MotorSpeedSensitivity = 2.6f;
        private const float MotorSteerSensitivity = 0.9f;

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

        private void JoyPanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            // multiply by MotorSpeedMultiplier to adjust the slider sensitivity
            float actualMotorSpeed = (float)(e.TotalY * MotorSpeedSensitivity);
            float actualMotorSteer = (float)(e.TotalX * MotorSteerSensitivity);
            int rawTranslationY = (int)e.TotalY;
            int rawTranslationX = (int)e.TotalX;

            if (actualMotorSpeed > 255 || actualMotorSpeed < -255)
            {
                actualMotorSpeed = Math.Clamp(actualMotorSpeed, -255, 255);
                rawTranslationY = (int)(actualMotorSpeed / MotorSpeedSensitivity);
            }

            if (actualMotorSteer > 100 || actualMotorSteer < -100)
            {
                actualMotorSteer = Math.Clamp(actualMotorSteer, -100, 100);
                rawTranslationX = (int)(actualMotorSteer / MotorSteerSensitivity);
            }

            Joystick_Stick.TranslationY = rawTranslationY;
            Joystick_Stick.TranslationX = rawTranslationX;
            // Set Motor steer angle (Round value correctly)

            int previousMotorSpeed = MotorSpeed;
            int previousMotorSteer = MotorSteerAngle;

            if ((previousMotorSpeed != (int)Math.Round(actualMotorSpeed) && System.Math.Abs(previousMotorSpeed - (int)Math.Round(actualMotorSpeed)) > 5) || (int)Math.Round(actualMotorSpeed) == 0)
            {
                MotorSpeed = (int)Math.Round(actualMotorSpeed);
            }

            if ((previousMotorSteer != (int)Math.Round(actualMotorSteer) && System.Math.Abs(previousMotorSteer - (int)Math.Round(actualMotorSteer)) > 5) || (int)Math.Round(actualMotorSteer) == 0)
            {
                MotorSteerAngle = (int)Math.Round(actualMotorSteer);
            }

            //Debug.Write($"MotorSpeed: {MotorSpeed}; RAW: {e.TotalY} ||||");
            //Debug.WriteLine($"MotorSteer: {MotorSteerAngle}; RAW: {e.TotalX}");

            // Send the data to the car
            Services.IBluetoothService bluetoothService = DependencyService.Get<Services.IBluetoothService>();
            if (previousMotorSpeed != MotorSpeed || MotorSpeed == 0)
            {
                Debug.Write($"MotorSpeed: {MotorSpeed}; RAW: {e.TotalY} ||||");
                bluetoothService.SendData(new Services.RCData { Type = Services.RCDataType.Speed, value = MotorSpeed });
            }

            if (previousMotorSteer != MotorSteerAngle || MotorSteerAngle == 0)
            {
                bluetoothService.SendData(new Services.RCData { Type = Services.RCDataType.Steer, value = MotorSteerAngle });
                Debug.WriteLine($"MotorSteer: {MotorSteerAngle}; RAW: {e.TotalX}");
            }
        }
    }

}
