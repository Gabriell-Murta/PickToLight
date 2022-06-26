using FluentValidation;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Shared;
using Stronzo.PickToLight.Api.Borders.UseCases.Product;
using Stronzo.PickToLight.Api.Borders.Validators;

namespace Stronzo.PickToLight.Api.UseCases.Product;

public class GetProductsUseCase : IGetProductsUseCase
{
    private readonly IProductRepository _productRepository;
    private readonly GetProductsRequestValidator _getProductsRequestValidator;

    public GetProductsUseCase(IProductRepository productRepository, GetProductsRequestValidator getProductsRequestValidator)
    {
        _productRepository = productRepository;
        _getProductsRequestValidator = getProductsRequestValidator;
    }

    public async Task<UseCaseResponse<IEnumerable<ProductResponse>>> Execute(GetProductsRequest request)
    {
        _getProductsRequestValidator.ValidateAndThrow(request);

        var products = await _productRepository.GetProductsByClientId(request.ClientId);

        var response = products.Select(product => product.ToProductResponse());

        return UseCaseResponse<IEnumerable<ProductResponse>>.Success(response);
    }
}
