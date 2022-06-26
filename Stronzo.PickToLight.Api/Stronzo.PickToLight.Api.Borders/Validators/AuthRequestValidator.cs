using FluentValidation;
using Stronzo.PickToLight.Api.Borders.Dtos;

namespace Stronzo.PickToLight.Api.Borders.Validators;

public class AuthRequestValidator : AbstractValidator<AuthRequest>
{
    public AuthRequestValidator()
    {
        RuleFor(x => x.Email).NotEmpty()
            .WithMessage("E-mail can't be null.");
        RuleFor(x => x.Password).NotEmpty()
            .WithMessage("Password can't be null.");
    }
}
