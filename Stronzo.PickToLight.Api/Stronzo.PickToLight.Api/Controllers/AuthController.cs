using Microsoft.AspNetCore.Mvc;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.UseCases.Auth;
using Stronzo.PickToLight.Api.Models;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IActionResultConverter _actionResultConverter;
    private readonly IAuthenticationUseCase _authenticationUseCase;

    public AuthController(IActionResultConverter actionResultConverter, IAuthenticationUseCase authenticationUseCase)
    {
        _actionResultConverter = actionResultConverter;
        _authenticationUseCase = authenticationUseCase;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorMessage[]))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage[]))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorMessage[]))]
    public async Task<IActionResult> Authenticate([FromBody] AuthRequest request)
    {
        var response = await _authenticationUseCase.Execute(request);
        return _actionResultConverter.Convert(response);
    }
}
