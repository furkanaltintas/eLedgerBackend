namespace Application.Features.Auth.Login;

public record LoginCommandResponse(string Token, string RefreshToken, DateTime RefreshTokenExpires);