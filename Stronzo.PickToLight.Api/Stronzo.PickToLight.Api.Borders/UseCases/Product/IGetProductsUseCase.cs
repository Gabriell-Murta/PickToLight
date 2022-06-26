using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Shared.Helpers;

namespace Stronzo.PickToLight.Api.Borders.UseCases.Product
{
    public interface IGetProductsUseCase : IUseCase<GetProductsRequest, IEnumerable<ProductResponse>> { }
}
