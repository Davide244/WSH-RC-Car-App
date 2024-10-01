using CommunityToolkit.Maui;
using MauiIcons.SegoeFluent;
using Microsoft.Extensions.Logging;

namespace WSHRCCarController
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentIcons");
                })
                //.ConfigureEffects(effects =>
                //{
                //    effects.InitFreakyEffects();
                //})
                .UseMauiCommunityToolkit();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.UseSegoeFluentMauiIcons();

            DependencyService.Register<Services.IBluetoothService, WSHRCCarController.Platforms.Android.Bluetooth.BluetoothConnector>();

            return builder.Build();
        }
    }
}
