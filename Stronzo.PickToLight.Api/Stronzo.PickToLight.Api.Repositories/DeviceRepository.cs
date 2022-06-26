using System.Data;
using Dapper;
using Stronzo.PickToLight.Api.Borders.Entities;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Repositories.Helpers;

namespace Stronzo.PickToLight.Api.Repositories;

public class DeviceRepository : IDeviceRepository
{
    private readonly IRepositoryHelper _helper;

    public DeviceRepository(IRepositoryHelper helper)
    {
        _helper = helper;
    }

    public async Task<IEnumerable<Device>> GetDevicesByClientId(Guid clientId)
    {
        const string sql = @"SELECT * FROM public.dispositivo
                                WHERE cliente_uuid = @clientId";

        using IDbConnection connection = _helper.GetConnection();

        return await connection.QueryAsync<Device>(sql, new { clientId = clientId });
    }
}

