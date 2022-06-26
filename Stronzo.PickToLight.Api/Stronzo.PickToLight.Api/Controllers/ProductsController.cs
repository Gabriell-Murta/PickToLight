using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.UseCases.Product;
using Stronzo.PickToLight.Api.Models;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IActionResultConverter _actionResultConverter;
        private readonly IGetProductsUseCase _getProductsUseCase;

        public ProductsController(IActionResultConverter actionResultConverter, IGetProductsUseCase getProductsUseCase)
        {
            _actionResultConverter = actionResultConverter;
            _getProductsUseCase = getProductsUseCase;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage[]))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorMessage[]))]
        public async Task<IActionResult> GetDevices([FromQuery] GetProductsRequest request)
        {
            var response = await _getProductsUseCase.Execute(request);
            return _actionResultConverter.Convert(response);
        }
    }
}
