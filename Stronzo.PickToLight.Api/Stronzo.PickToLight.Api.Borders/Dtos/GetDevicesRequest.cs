namespace Stronzo.PickToLight.Api.Borders.Dtos
{
    public record GetDevicesRequest
    {
        public Guid ClientId { get; init; }
    }
}
