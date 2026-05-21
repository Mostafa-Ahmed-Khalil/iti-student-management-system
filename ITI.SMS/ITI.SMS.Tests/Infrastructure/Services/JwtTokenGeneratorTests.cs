using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ITI.SMS.Domain.Entities;
using ITI.SMS.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using NSubstitute;

namespace ITI.SMS.Tests.Infrastructure.Services;

public class JwtTokenGeneratorTests
{
    private readonly IConfiguration _configuration;
    private readonly JwtTokenGenerator _generator;

    public JwtTokenGeneratorTests()
    {
        _configuration = Substitute.For<IConfiguration>();
        
        // Mock the configuration values
        _configuration["Jwt:Secret"].Returns("A_Very_Long_And_Super_Secure_Secret_Key_At_Least_32_Bytes_Long");
        _configuration["Jwt:Issuer"].Returns("ITI.SMS");
        _configuration["Jwt:Audience"].Returns("ITI.SMS.Client");
        _configuration["Jwt:ExpiryInHours"].Returns("8");

        _generator = new JwtTokenGenerator(_configuration);
    }

    [Fact]
    public void GenerateToken_WithValidUserAndRoles_ShouldGenerateTokenWithCorrectClaimsAndExpiration()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Id = "user-123",
            Email = "test@iti.edu"
        };
        var roles = new List<string> { "Admin", "Instructor" };

        // Act
        var (token, expiresAt) = _generator.GenerateToken(user, roles);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        
        // Verify token structure
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);

        Assert.Equal("ITI.SMS", jwtToken.Issuer);
        Assert.Equal("ITI.SMS.Client", jwtToken.Audiences.First());
        
        // Verify claims
        var claims = jwtToken.Claims.ToList();
        
        var userIdClaim = claims.FirstOrDefault(c => c.Type == "userId");
        Assert.NotNull(userIdClaim);
        Assert.Equal(user.Id, userIdClaim.Value);

        var emailClaim = claims.FirstOrDefault(c => c.Type == "email");
        Assert.NotNull(emailClaim);
        Assert.Equal(user.Email, emailClaim.Value);

        // Verify roles claims
        // Verify roles claims
        var roleClaims = claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList();
        Assert.Contains("Admin", roleClaims);
        Assert.Contains("Instructor", roleClaims);
        
        var stdRoleClaims = claims.Where(c => c.Type == "role").Select(c => c.Value).ToList();
        Assert.Contains("Admin", stdRoleClaims);
        Assert.Contains("Instructor", stdRoleClaims);

        // Verify expiration is around 8 hours from now
        var timeDifference = expiresAt - DateTime.UtcNow;
        Assert.True(timeDifference.TotalHours > 7.9 && timeDifference.TotalHours <= 8.0);
    }
}
