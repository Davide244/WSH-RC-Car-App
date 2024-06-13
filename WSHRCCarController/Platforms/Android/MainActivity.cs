using Android.App;
using Android.Content.PM;
using Android.OS;

namespace WSHRCCarController
{

    public enum DeviceOrientation
    {
        Landscape,
        Portrait
    }

    [IntentFilter(new[] { Platform.Intent.ActionAppAction }, Categories = new[] { Android.Content.Intent.CategoryDefault })]
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public sealed class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            RequestedOrientation = ScreenOrientation.Landscape;

            if (AskBluetoothPermission().Equals(PermissionStatus.Granted) == false)
            {
                // Close the app if the user denies the Bluetooth permission
                //Finish();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();

            Platform.OnResume(this);
        }

        protected override void OnNewIntent(Android.Content.Intent intent)
        {
            base.OnNewIntent(intent);

            Platform.OnNewIntent(intent);
        }

        public void SetOrientation(DeviceOrientation orientation)
        {
            RequestedOrientation = orientation switch
            {
                DeviceOrientation.Landscape => ScreenOrientation.Landscape,
                DeviceOrientation.Portrait => ScreenOrientation.Portrait,
                _ => ScreenOrientation.Unspecified
            };
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private async Task<PermissionStatus> AskBluetoothPermission() 
        {
            if ((await Permissions.CheckStatusAsync<Permissions.Bluetooth>()).Equals(PermissionStatus.Granted))
            {
                return PermissionStatus.Granted;
            }
            else
            {
                return await Permissions.RequestAsync<Permissions.Bluetooth>();
            }
        }
    }
}
