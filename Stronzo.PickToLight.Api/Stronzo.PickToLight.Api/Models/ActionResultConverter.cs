using Microsoft.AspNetCore.Mvc;
using Stronzo.PickToLight.Api.Borders.Shared;
using Stronzo.PickToLight.Api.Shared.Models;
using System.Net;

namespace Stronzo.PickToLight.Api.Models;

public interface IActionResultConverter
{
    IActionResult Convert<T>(UseCaseResponse<T> response, bool noContentIfSuccess = false);
}

public class ActionResultConverter : IActionResultConverter
{
    public IActionResult Convert<T>(UseCaseResponse<T>? response, bool noContentIfSuccess = false)
    {
        if (response == null)
            return BuildError(new[] { new ErrorMessage("000", "ActionResultConverter Error") }, UseCaseResponseKind.InternalServerError);

        if (response.ErrorMessage is null)
        {
            if (noContentIfSuccess)
            {
                return new NoContentResult();
            }
            else
            {
                return BuildSuccessResult(response.Result!);
            }
        }
        else if (response.Result != null)
        {
            return BuildError(response.Result, response.Status);
        }
        else
        {
            var hasErrors = response.Errors == null || !response.Errors.Any();
            var errorResult = hasErrors
                ? new[] { new ErrorMessage("000", response.ErrorMessage ?? "Unknown error") }
                : response.Errors;

            return BuildError(errorResult!, response.Status);
        }
    }

    private static IActionResult BuildSuccessResult(object data)
    {
        return new OkObjectResult(data);
    }

    private static ObjectResult BuildError(object data, UseCaseResponseKind status)
    {
        var httpStatus = GetErrorHttpStatusCode(status);

        return new ObjectResult(data)
        {
            StatusCode = (int)httpStatus
        };
    }

    private static HttpStatusCode GetErrorHttpStatusCode(UseCaseResponseKind status)
    {
        switch (status)
        {
            case UseCaseResponseKind.RequestValidationError:
            case UseCaseResponseKind.ForeignKeyViolationError:
            case UseCaseResponseKind.BadRequest:
                return HttpStatusCode.BadRequest;
            case UseCaseResponseKind.Unauthorized:
                return HttpStatusCode.Unauthorized;
            case UseCaseResponseKind.Forbidden:
                return HttpStatusCode.Forbidden;
            case UseCaseResponseKind.NotFound:
                return HttpStatusCode.NotFound;
            case UseCaseResponseKind.UniqueViolationError:
                return HttpStatusCode.Conflict;
            case UseCaseResponseKind.Unavailable:
                return HttpStatusCode.ServiceUnavailable;
            case UseCaseResponseKind.BadGateway:
                return HttpStatusCode.BadGateway;
            case UseCaseResponseKind.UnprocessableEntity:
                return HttpStatusCode.UnprocessableEntity;
            default:
                return HttpStatusCode.InternalServerError;
        }
    }
}
