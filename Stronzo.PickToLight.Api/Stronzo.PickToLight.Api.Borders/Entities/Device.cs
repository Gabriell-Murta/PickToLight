#nullable disable

using Stronzo.PickToLight.Api.Borders.Dtos;

namespace Stronzo.PickToLight.Api.Borders.Entities;

public record Device
{
    public Guid Dispositivo_Uuid { get; init; }
    public Guid Cliente_Uuid { get; init; }
    public Guid Produto_Uuid { get; init; }

    public DeviceResponse ToDeviceResponse() =>
        new DeviceResponse
        {
            Id = Dispositivo_Uuid
        };
}
