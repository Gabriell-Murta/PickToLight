using Dapper;
using Stronzo.PickToLight.Api.Borders.Entities;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Repositories.Helpers;
using System.Data;

namespace Stronzo.PickToLight.Api.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IRepositoryHelper _helper;

    public UserRepository(IRepositoryHelper helper)
    {
        _helper = helper;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        const string sql = @"SELECT * FROM public.usuario
                                WHERE email = @email";

        using IDbConnection connection = _helper.GetConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { email = email });
    }

    public async Task<User> GetUserById(Guid id)
    {
        const string sql = @"SELECT * FROM public.usuario
                                WHERE usuario_Uuid = @id";

        using IDbConnection connection = _helper.GetConnection();

        return await connection.QueryFirstOrDefaultAsync<User>(sql, new { id = id });
    }
}
