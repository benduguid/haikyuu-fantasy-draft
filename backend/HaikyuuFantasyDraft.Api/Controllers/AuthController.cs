using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HaikyuuFantasyDraft.Api.Models;
using HaikyuuFantasyDraft.Api.Models.Auth;
using HaikyuuFantasyDraft.Api.Services;

namespace HaikyuuFantasyDraft.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly JwtService _jwtService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        JwtService jwtService,
        ILogger<AuthController> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { message = "User with this email already exists" });
            }

            // Create new user
            var user = new User
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new { 
                    message = "Registration failed", 
                    errors = result.Errors.Select(e => e.Description) 
                });
            }

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);
            var expiresAt = _jwtService.GetTokenExpiration();

            return Ok(new AuthResponse
            {
                Token = token,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                ExpiresAt = expiresAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            // Update last login time
            user.LastLoginAt = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            // Generate JWT token
            var token = _jwtService.GenerateToken(user);
            var expiresAt = _jwtService.GetTokenExpiration();

            return Ok(new AuthResponse
            {
                Token = token,
                Email = user.Email!,
                DisplayName = user.DisplayName,
                ExpiresAt = expiresAt
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpPost("test")]
    public IActionResult Test()
    {
        return Ok(new { 
            message = "Auth endpoints are working!", 
            timestamp = DateTime.UtcNow 
        });
    }
}