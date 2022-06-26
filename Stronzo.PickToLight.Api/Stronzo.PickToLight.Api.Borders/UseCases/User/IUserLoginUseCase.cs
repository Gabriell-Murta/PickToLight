using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Shared.Helpers;

namespace Stronzo.PickToLight.Api.Borders.UseCases.User
{
    public interface IGetUserByIdUseCase : IUseCase<Guid, UserResponse> { }
}
