using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HaikyuuFantasyDraft.Api.Models;

public class Team
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public Difficulty Difficulty { get; set; }
    
    public int TotalPointCost { get; set; }
    
    public bool IsComplete { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign key
    [Required]
    public string UserId { get; set; } = string.Empty;
    
    // Navigation properties
    [ForeignKey("UserId")]
    public User User { get; set; } = null!;
    
    public ICollection<TeamPlayer> TeamPlayers { get; set; } = new List<TeamPlayer>();
}

// Junction table for many-to-many relationship
public class TeamPlayer
{
    public int Id { get; set; }
    
    [Required]
    public int TeamId { get; set; }
    
    [Required]
    public int PlayerId { get; set; }
    
    [Required]
    public Position AssignedPosition { get; set; }
    
    public int DraftOrder { get; set; }
    
    // Navigation properties
    [ForeignKey("TeamId")]
    public Team Team { get; set; } = null!;
    
    [ForeignKey("PlayerId")]
    public Player Player { get; set; } = null!;
}

public enum Difficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3
}