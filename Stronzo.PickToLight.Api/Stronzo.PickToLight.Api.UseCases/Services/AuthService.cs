using System.Text;
using System.Security.Claims;
using Stronzo.PickToLight.Api.Borders.Services;
using Stronzo.PickToLight.Api.Shared.Configurations;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Stronzo.PickToLight.Api.UseCases.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationConfig _appConfig;

    public AuthService(ApplicationConfig appConfig)
    {
        _appConfig = appConfig;
    }

    public string GenerateToken(Borders.Entities.User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appConfig.Authentication.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Email.ToString())
            }),
            Expires = DateTime.UtcNow.AddHours(2),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
