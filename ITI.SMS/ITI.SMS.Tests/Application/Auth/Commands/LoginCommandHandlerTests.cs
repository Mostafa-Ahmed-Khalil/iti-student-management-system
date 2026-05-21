using ITI.SMS.Application.Auth.Commands;
using ITI.SMS.Application.Common.Exceptions;
using ITI.SMS.Application.Common.Interfaces;
using ITI.SMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using NSubstitute;

namespace ITI.SMS.Tests.Application.Auth.Commands;

public class LoginCommandHandlerTests
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        // Mock user store which is required by UserManager constructor
        var userStore = Substitute.For<IUserStore<ApplicationUser>>();
        
        // Mock UserManager directly using NSubstitute
        _userManager = Substitute.For<UserManager<ApplicationUser>>(
            userStore, null, null, null, null, null, null, null, null);

        _jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

        _handler = new LoginCommandHandler(_userManager, _jwtTokenGenerator);
    }

    [Fact]
    public async Task Handle_WithValidCredentials_ShouldReturnTokenAndExpiration()
    {
        // Arrange
        var email = "user@iti.edu";
        var password = "Password123";
        var user = new ApplicationUser { Email = email, FullName = "Test User" };
        var roles = new List<string> { "Student" };
        var token = "mocked-jwt-token";
        var expiresAt = DateTime.UtcNow.AddHours(8);

        var command = new LoginCommand(email, password);

        _userManager.FindByEmailAsync(email).Returns(user);
        _userManager.CheckPasswordAsync(user, password).Returns(true);
        _userManager.GetRolesAsync(user).Returns(roles);
        
        _jwtTokenGenerator.GenerateToken(user, Arg.Any<IList<string>>()).Returns((token, expiresAt));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(token, result.Token);
        Assert.Equal(expiresAt, result.ExpiresAt);
    }

    [Fact]
    public async Task Handle_WithNonExistentUser_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var email = "nonexistent@iti.edu";
        var password = "Password123";
        var command = new LoginCommand(email, password);

        _userManager.FindByEmailAsync(email).Returns((ApplicationUser?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithInvalidPassword_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var email = "user@iti.edu";
        var password = "WrongPassword";
        var user = new ApplicationUser { Email = email };
        var command = new LoginCommand(email, password);

        _userManager.FindByEmailAsync(email).Returns(user);
        _userManager.CheckPasswordAsync(user, password).Returns(false);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }
}
