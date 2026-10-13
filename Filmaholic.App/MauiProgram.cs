using Filmaholic.App.Components;
using Filmaholic.App.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text.Json;

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

        using var appSettingsStream = Assembly.GetExecutingAssembly()
            .GetManifestResourceStream("Filmaholic.App.appsettings.json")
            ?? throw new InvalidOperationException("The bundled API settings file is missing.");
        using var appSettings = JsonDocument.Parse(appSettingsStream);

#if DEBUG
        var configuredBaseAddress = OperatingSystem.IsAndroid()
            ? "https://10.0.2.2:7047/"
            : "https://localhost:7047/";
#else
    var configuredBaseAddress = appSettings.RootElement
        .GetProperty("Api")
        .GetProperty("BaseUrl")
        .GetString();
#endif

        if (!Uri.TryCreate(configuredBaseAddress, UriKind.Absolute, out var apiBaseAddress) ||
            apiBaseAddress.Scheme != Uri.UriSchemeHttps ||
            apiBaseAddress.Host.EndsWith(".invalid", StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                "Set Api:BaseUrl in Filmaholic.App/appsettings.json to the deployed HTTPS API before publishing.");
        }

        builder.Services.AddSingleton<TokenStore>();
        builder.Services.AddSingleton<AuthenticationStateNotifier>();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddTransient<AuthMessageHandler>();
        builder.Services.AddHttpClient("FilmaholicApi", client =>
        {
            client.BaseAddress = apiBaseAddress;
        })
        .AddHttpMessageHandler<AuthMessageHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("FilmaholicApi"));

        // Register MovieService
        builder.Services.AddScoped<MovieService>();

        return builder.Build();
    }
}