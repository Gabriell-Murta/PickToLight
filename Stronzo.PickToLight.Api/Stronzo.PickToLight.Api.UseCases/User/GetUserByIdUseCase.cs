using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Shared;
using Stronzo.PickToLight.Api.Borders.UseCases.User;

namespace Stronzo.PickToLight.Api.UseCases.User;

public class GetUserByIdUseCase : IGetUserByIdUseCase
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdUseCase(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UseCaseResponse<UserResponse>> Execute(Guid request)
    {
        var user = await _userRepository.GetUserById(request);

        if (user is null)
            return UseCaseResponse<UserResponse>.NotFound("User not found.");

        return UseCaseResponse<UserResponse>.Success(user.ToUserResponse());
    }
}
