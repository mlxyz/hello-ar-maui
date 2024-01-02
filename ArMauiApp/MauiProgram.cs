using ArMauiApp.Controls;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using static Android.Provider.MediaStore;

namespace ArMauiApp
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
                })
            .ConfigureMauiHandlers(handlers =>
            {
                    handlers.AddHandler(typeof(ArView), typeof(ArViewHandler));
                }); ;

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
