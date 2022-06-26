namespace Stronzo.PickToLight.Api.Borders.Dtos;

public record GetProductsRequest
{
    public Guid ClientId { get; init; }
}
