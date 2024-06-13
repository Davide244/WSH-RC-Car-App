using CommunityToolkit.Maui;
using MauiIcons.SegoeFluent;
using Microsoft.Extensions.Logging;
using Shiny;

namespace WSHRCCarController
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseShiny()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("FluentSystemIcons-Regular.ttf", "FluentIcons");
                })
                .UseMauiCommunityToolkit();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.UseSegoeFluentMauiIcons();
            builder.Services.AddBluetoothLE();

            DependencyService.Register<Services.IBluetoothService, WSHRCCarController.Platforms.Android.Bluetooth.BluetoothConnector>();

            return builder.Build();
        }
    }
}
