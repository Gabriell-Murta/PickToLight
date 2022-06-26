#nullable disable

namespace Stronzo.PickToLight.Api.Borders.Dtos;

public record ProductResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Code { get; init; }
}

