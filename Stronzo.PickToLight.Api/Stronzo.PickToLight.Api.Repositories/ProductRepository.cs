using System.Data;
using Dapper;
using Stronzo.PickToLight.Api.Borders.Entities;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Repositories.Helpers;

namespace Stronzo.PickToLight.Api.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IRepositoryHelper _helper;

    public ProductRepository(IRepositoryHelper helper)
    {
        _helper = helper;
    }

    public async Task<IEnumerable<Product>> GetProductsByClientId(Guid clientId)
    {
        const string sql = @"SELECT * FROM public.produto
                                WHERE cliente_uuid = @clientId";

        using IDbConnection connection = _helper.GetConnection();

        return await connection.QueryAsync<Product>(sql, new { clientId = clientId });
    }
}
