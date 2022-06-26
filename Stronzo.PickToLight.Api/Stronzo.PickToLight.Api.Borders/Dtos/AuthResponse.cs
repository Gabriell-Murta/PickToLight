#nullable disable

namespace Stronzo.PickToLight.Api.Borders.Dtos;

public record AuthResponse
{
    public Guid UserId { get; init; }
    public string Token { get; init; }
}

