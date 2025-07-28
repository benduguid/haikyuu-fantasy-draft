using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HaikyuuFantasyDraft.Api.Data;
using HaikyuuFantasyDraft.Api.Models;

namespace HaikyuuFantasyDraft.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlayersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PlayersController> _logger;

    public PlayersController(ApplicationDbContext context, ILogger<PlayersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
    {
        try
        {
            var players = await _context.Players.ToListAsync();
            return Ok(players);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving players");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        try
        {
            var player = await _context.Players.FindAsync(id);
            
            if (player == null)
            {
                return NotFound();
            }

            return Ok(player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving player {PlayerId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(Player player)
    {
        try
        {
            player.CreatedAt = DateTime.UtcNow;
            player.UpdatedAt = DateTime.UtcNow;
            
            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = player.Id }, player);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating player");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("test")]
    public IActionResult Test()
    {
        return Ok(new { 
            message = "Haikyuu Fantasy Draft API is working!", 
            timestamp = DateTime.UtcNow,
            database = "Connected to PostgreSQL"
        });
    }
}