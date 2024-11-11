using Microsoft.Extensions.Logging;
using HouseBouncer.Services;
using HouseBouncer.ViewModels;
using HouseBouncer.Views;

namespace HouseBouncer
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
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register services
            builder.Services.AddSingleton<DataService>();
            builder.Services.AddTransient<IDialogService, DialogService>();

            // Register ViewModels
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<RoomViewModel>();
            builder.Services.AddTransient<DeviceViewModel>();

            // Register Pages (if you want to resolve them via DI)
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<RoomPage>();
            builder.Services.AddTransient<DevicePage>();

            // Register AppShell
            builder.Services.AddSingleton<AppShell>();

            return builder.Build();
        }
    }
}
