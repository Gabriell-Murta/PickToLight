using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.UseCases.Device;
using Stronzo.PickToLight.Api.Models;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly IActionResultConverter _actionResultConverter;
        private readonly IGetDevicesUseCase _getDevicesUseCase;

        public DevicesController(IActionResultConverter actionResultConverter, IGetDevicesUseCase getDevicesUseCase)
        {
            _actionResultConverter = actionResultConverter;
            _getDevicesUseCase = getDevicesUseCase;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DeviceResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage[]))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage[]))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorMessage[]))]
        public async Task<IActionResult> GetDevices([FromQuery] GetDevicesRequest request)
        {
            var response = await _getDevicesUseCase.Execute(request);
            return _actionResultConverter.Convert(response);
        }
    }
}
