using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HaikyuuFantasyDraft.Api.Models;

public class User : IdentityUser
{
    [MaxLength(100)]
    public string? DisplayName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime LastLoginAt { get; set; }
    
    // Navigation properties
    public ICollection<Team> Teams { get; set; } = new List<Team>();
}