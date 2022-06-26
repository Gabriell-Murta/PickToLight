using Stronzo.PickToLight.Api.Borders.Entities;

namespace Stronzo.PickToLight.Api.Borders.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProductsByClientId(Guid clientId);
}
