using Npgsql;
using Stronzo.PickToLight.Api.Borders.Repositories.Helpers;
using Stronzo.PickToLight.Api.Shared.Configurations;
using System.Data;

namespace Stronzo.PickToLight.Api.Repositories.Helpers;

public class RepositoryHelper : IRepositoryHelper
{
    private readonly ApplicationConfig _appConfig;

    public RepositoryHelper(ApplicationConfig appConfig)
    {
        _appConfig = appConfig;
    }

    public IDbConnection GetConnection()
    {
        return new NpgsqlConnection(_appConfig.ConnectionStrings.DefaultConnection);
    }
}
