namespace Filmaholic.Shared.Dtos;

public sealed class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}
