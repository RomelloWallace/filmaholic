namespace Filmaholic.App.Services;

public sealed class AuthenticationStateNotifier
{
    public event Action? AuthenticationStateChanged;

    public void NotifyAuthenticationStateChanged()
    {
        AuthenticationStateChanged?.Invoke();
    }
}