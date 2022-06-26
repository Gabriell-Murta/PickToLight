using Stronzo.PickToLight.Api.Borders.Entities;

namespace Stronzo.PickToLight.Api.Borders.Repositories;

public interface IDeviceRepository
{
    Task<IEnumerable<Device>> GetDevicesByClientId(Guid clientId);
}
