using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HaikyuuFantasyDraft.Api.Models;

namespace HaikyuuFantasyDraft.Api.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamPlayer> TeamPlayers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Player configuration
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
            entity.Property(p => p.School).HasMaxLength(100);
            entity.Property(p => p.Description).HasMaxLength(500);
            entity.Property(p => p.Position).HasConversion<int>();
            
            // Ensure stats are within valid ranges using newer syntax
            entity.ToTable(t => 
            {
                t.HasCheckConstraint("CK_Player_Power", "\"Power\" >= 1 AND \"Power\" <= 100");
                t.HasCheckConstraint("CK_Player_Jumping", "\"Jumping\" >= 1 AND \"Jumping\" <= 100");
                t.HasCheckConstraint("CK_Player_Stamina", "\"Stamina\" >= 1 AND \"Stamina\" <= 100");
                t.HasCheckConstraint("CK_Player_GameSense", "\"GameSense\" >= 1 AND \"GameSense\" <= 100");
                t.HasCheckConstraint("CK_Player_Technique", "\"Technique\" >= 1 AND \"Technique\" <= 100");
                t.HasCheckConstraint("CK_Player_Speed", "\"Speed\" >= 1 AND \"Speed\" <= 100");
                t.HasCheckConstraint("CK_Player_PointCost", "\"PointCost\" >= 1 AND \"PointCost\" <= 50");
            });
        });

        // Team configuration
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Name).IsRequired().HasMaxLength(100);
            entity.Property(t => t.Description).HasMaxLength(500);
            entity.Property(t => t.Difficulty).HasConversion<int>();
            entity.Property(t => t.UserId).IsRequired();
            
            entity.HasOne(t => t.User)
                  .WithMany(u => u.Teams)
                  .HasForeignKey(t => t.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // TeamPlayer configuration (junction table)
        modelBuilder.Entity<TeamPlayer>(entity =>
        {
            entity.HasKey(tp => tp.Id);
            entity.Property(tp => tp.AssignedPosition).HasConversion<int>();
            
            entity.HasOne(tp => tp.Team)
                  .WithMany(t => t.TeamPlayers)
                  .HasForeignKey(tp => tp.TeamId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            entity.HasOne(tp => tp.Player)
                  .WithMany(p => p.TeamPlayers)
                  .HasForeignKey(tp => tp.PlayerId)
                  .OnDelete(DeleteBehavior.Restrict);

            // Ensure a player can only be on a team once
            entity.HasIndex(tp => new { tp.TeamId, tp.PlayerId })
                  .IsUnique();
        });

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.DisplayName).HasMaxLength(100);
        });
    }
}