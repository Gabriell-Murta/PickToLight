using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.UseCases.User;
using Stronzo.PickToLight.Api.Models;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IActionResultConverter _actionResultConverter;
    private readonly IGetUserByIdUseCase _getUserByIdUseCase;

    public UsersController(IGetUserByIdUseCase getUserByIdUseCase, IActionResultConverter actionResultConverter)
    {
        _getUserByIdUseCase = getUserByIdUseCase;
        _actionResultConverter = actionResultConverter;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorMessage[]))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorMessage[]))]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var response = await _getUserByIdUseCase.Execute(id);
        return _actionResultConverter.Convert(response);
    }
}
