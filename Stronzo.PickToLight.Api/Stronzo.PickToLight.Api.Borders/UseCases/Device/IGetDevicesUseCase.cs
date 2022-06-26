using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Shared.Helpers;

namespace Stronzo.PickToLight.Api.Borders.UseCases.Device
{
    public interface IGetDevicesUseCase : IUseCase<GetDevicesRequest, IEnumerable<DeviceResponse>> { }
}
