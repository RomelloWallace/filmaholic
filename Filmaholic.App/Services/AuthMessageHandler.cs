using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Filmaholic.App.Services;

public sealed class AuthMessageHandler : DelegatingHandler
{
    private readonly TokenStore _tokenStore;

    public AuthMessageHandler(TokenStore tokenStore)
    {
        _tokenStore = tokenStore;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_tokenStore.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _tokenStore.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
