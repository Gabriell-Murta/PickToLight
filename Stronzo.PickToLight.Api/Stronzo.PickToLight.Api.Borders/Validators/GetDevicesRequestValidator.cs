using FluentValidation;
using Stronzo.PickToLight.Api.Borders.Dtos;
using Stronzo.PickToLight.Api.Shared.Constants;

namespace Stronzo.PickToLight.Api.Borders.Validators;

public class GetDevicesRequestValidator : AbstractValidator<GetDevicesRequest>
{
    public GetDevicesRequestValidator()
    {
        RuleFor(request => request.ClientId).NotEmpty().WithErrorCode(ErrorCodes.BadRequest);
    }
}
