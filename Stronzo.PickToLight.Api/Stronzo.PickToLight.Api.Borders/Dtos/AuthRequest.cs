#nullable disable

namespace Stronzo.PickToLight.Api.Borders.Dtos;

public record AuthRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}
