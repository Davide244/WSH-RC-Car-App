﻿using Android.App;
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
    }
}
