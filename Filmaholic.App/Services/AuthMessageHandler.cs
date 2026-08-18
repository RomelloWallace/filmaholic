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

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenStore.GetTokenAsync();
        
        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
