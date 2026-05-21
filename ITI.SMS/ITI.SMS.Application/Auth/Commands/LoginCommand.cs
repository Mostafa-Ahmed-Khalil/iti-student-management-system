using MediatR;

namespace ITI.SMS.Application.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<LoginCommandResponse>;
