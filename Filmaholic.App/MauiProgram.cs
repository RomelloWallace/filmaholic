using Filmaholic.App.Components;
using Filmaholic.App.Services;
using Microsoft.Extensions.Logging;

namespace Filmaholic.App;

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
            });
            
		builder.Services.AddMauiBlazorWebView();
#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

        // Register HttpClient with base API address
        builder.Services.AddScoped(sp =>
            new HttpClient
            {
                BaseAddress = new Uri("http://127.0.0.1:5220/")
            });

        // Register MovieService
        builder.Services.AddScoped<MovieService>();

        return builder.Build();
    }
}