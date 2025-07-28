﻿namespace HaikyuuFantasyDraft.Api.Models.Auth;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? DisplayName { get; set; }
    public DateTime ExpiresAt { get; set; }
}