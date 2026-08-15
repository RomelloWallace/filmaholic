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

        // Register auth services and HttpClient with a delegating handler
        var baseAddress = OperatingSystem.IsAndroid()
            ? "http://10.0.2.2:5220/"
            : "http://localhost:5220/";

        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddTransient<AuthMessageHandler>();
        builder.Services.AddHttpClient("FilmaholicApi", client =>
        {
            client.BaseAddress = new Uri(baseAddress);
        })
        .AddHttpMessageHandler<AuthMessageHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("FilmaholicApi"));

        // Register MovieService
        builder.Services.AddScoped<MovieService>();

        return builder.Build();
    }
}