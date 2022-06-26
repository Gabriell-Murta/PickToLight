using FluentValidation;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Shared;
using Stronzo.PickToLight.Api.Borders.UseCases.Device;
using Stronzo.PickToLight.Api.Borders.Validators;

namespace Stronzo.PickToLight.Api.UseCases.Device
{
    public class GetDevicesUseCase : IGetDevicesUseCase
    {
        private readonly IDeviceRepository _deviceRepository;
        private readonly GetDevicesRequestValidator _getDevicesRequestValidator;

        public GetDevicesUseCase(
            IDeviceRepository deviceRepository,
            GetDevicesRequestValidator getDevicesRequestValidator)
        {
            _deviceRepository = deviceRepository;
            _getDevicesRequestValidator = getDevicesRequestValidator;
        }

        public async Task<UseCaseResponse<IEnumerable<DeviceResponse>>> Execute(GetDevicesRequest request)
        {
            _getDevicesRequestValidator.ValidateAndThrow(request);

            var devices = await _deviceRepository.GetDevicesByClientId(request.ClientId);

            var response = devices.Select(device => device.ToDeviceResponse());

            return UseCaseResponse<IEnumerable<DeviceResponse>>.Success(response);
        }
    }
}
