namespace ITI.SMS.Application.Auth.Commands;

public record LoginCommandResponse(string Token, DateTime ExpiresAt);
