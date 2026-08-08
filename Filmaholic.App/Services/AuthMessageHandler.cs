using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Filmaholic.App.Services;

public sealed class AuthMessageHandler : DelegatingHandler
{
    private readonly AuthService _authService;

    public AuthMessageHandler(AuthService authService)
    {
        _authService = authService;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_authService.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _authService.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
