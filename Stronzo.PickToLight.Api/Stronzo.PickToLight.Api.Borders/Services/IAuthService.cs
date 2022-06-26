using Stronzo.PickToLight.Api.Borders.Entities;

namespace Stronzo.PickToLight.Api.Borders.Services
{
    public interface IAuthService
    {
        string GenerateToken(User user);
    }
}
