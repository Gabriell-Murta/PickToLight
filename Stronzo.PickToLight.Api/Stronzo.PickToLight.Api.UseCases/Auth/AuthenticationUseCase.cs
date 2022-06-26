using FluentValidation;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Services;
using Stronzo.PickToLight.Api.Borders.Shared;
using Stronzo.PickToLight.Api.Borders.UseCases.Auth;
using Stronzo.PickToLight.Api.Borders.Validators;

namespace Stronzo.PickToLight.Api.UseCases.Auth;

public class AuthenticationUseCase : IAuthenticationUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _tokenService;
    private readonly AuthRequestValidator _authRequestValidator;


    public AuthenticationUseCase(
        IUserRepository userRepository,
        IAuthService tokenService,
        AuthRequestValidator authRequestValidator)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _authRequestValidator = authRequestValidator;
    }

    public async Task<UseCaseResponse<AuthResponse>> Execute(AuthRequest request)
    {
        _authRequestValidator.ValidateAndThrow(request);

        var user = await _userRepository.GetUserByEmail(request.Email);

        if (user is null)
            return UseCaseResponse<AuthResponse>.NotFound("User not found.");

        if (user.Token_Usuario == request.Password)
        {
            var response = new AuthResponse()
            {
                UserId = user.Usuario_Uuid,
                Token = _tokenService.GenerateToken(user)
            };

            return UseCaseResponse<AuthResponse>.Success(response);
        }

        return UseCaseResponse<AuthResponse>.BadRequest("Invalid password");
    }
}
