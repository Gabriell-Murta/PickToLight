using FluentValidation;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Shared.Extensions;

public static class ValidationExceptionExtensions
{
    public static IEnumerable<ErrorMessage> ToErrorsMessage(this ValidationException exception)
    {
        return exception.Errors.Select(error => new ErrorMessage(error.ErrorCode, error.ErrorMessage));
    }
}
