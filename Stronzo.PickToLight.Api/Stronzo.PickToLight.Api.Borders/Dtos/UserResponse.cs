#nullable disable

namespace Stronzo.PickToLight.Api.Borders.Dtos;

public class UserResponse
{
    public Guid Id { get; init; }
    public string Email { get; init; }
    public string Name { get; init; }
    public Guid ClientId { get; init; }
}
