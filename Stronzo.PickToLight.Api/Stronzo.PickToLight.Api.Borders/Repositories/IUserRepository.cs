using Stronzo.PickToLight.Api.Borders.Entities;

namespace Stronzo.PickToLight.Api.Borders.Repositories;

public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(Guid id);
}
