using Microsoft.Extensions.Logging;
using RealTalk.Maui.Services;

namespace RealTalk.Maui
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

            builder.Services.AddSingleton<ApiService>();
            builder.Services.AddSingleton<AuthService>();

            //builder.Services.AddHttpClient<ApiService>(client =>
            //{
            //    client.BaseAddress = new Uri("http://192.168.0.106:5123");
            //    client.Timeout = TimeSpan.FromSeconds(5);
            //});

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
