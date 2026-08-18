using Microsoft.AspNetCore.Identity;

namespace Filmaholic.Api.Entities;

public class UserEntity : IdentityUser<Guid>
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
}
