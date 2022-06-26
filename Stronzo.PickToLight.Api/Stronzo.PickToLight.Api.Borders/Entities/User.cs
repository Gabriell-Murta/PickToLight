#nullable disable

using Stronzo.PickToLight.Api.Borders.Dtos;

namespace Stronzo.PickToLight.Api.Borders.Entities;

public record User
{
    public Guid Usuario_Uuid { get; init; }
    public string Nome { get; init; }
    public string Email { get; init; }
    public string Token_Usuario { get; init; }
    public Guid Cliente_Uuid { get; init; }

    public UserResponse ToUserResponse() =>
        new()
        {
            Id = Usuario_Uuid,
            Name = Nome,
            Email = Email,
            ClientId = Cliente_Uuid
        };
}
