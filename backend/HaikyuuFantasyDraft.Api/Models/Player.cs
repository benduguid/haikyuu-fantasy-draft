using System.ComponentModel.DataAnnotations;

namespace HaikyuuFantasyDraft.Api.Models;

public class Player
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(100)]
    public string School { get; set; } = string.Empty;
    
    [Required]
    public Position Position { get; set; }
    
    [Range(1, 100)]
    public int Power { get; set; }
    
    [Range(1, 100)]
    public int Jumping { get; set; }
    
    [Range(1, 100)]
    public int Stamina { get; set; }
    
    [Range(1, 100)]
    public int GameSense { get; set; }
    
    [Range(1, 100)]
    public int Technique { get; set; }
    
    [Range(1, 100)]
    public int Speed { get; set; }
    
    [Range(1, 50)]
    public int PointCost { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
}

public enum Position
{
    MiddleBlocker = 1,
    WingSpiker = 2,
    Setter = 3,
    Libero = 4,
    Opposite = 5
}