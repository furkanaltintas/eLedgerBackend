using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class AppUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string FullName => string.Join(" ", FirstName, LastName);
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpires { get; set; }
    public bool IsDeleted { get; set; } = false;
    public List<CompanyUser>? CompanyUsers { get; set; }
    public bool IsAdmin { get; set; }
}