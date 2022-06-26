using Stronzo.PickToLight.Api.Shared.Constants;
using Stronzo.PickToLight.Api.Shared.Models;

namespace Stronzo.PickToLight.Api.Borders.Shared;

public class UseCaseResponse<T>
{
    public UseCaseResponseKind Status { get; private set; }
    public T? Result { get; private set; }
    public string? ResultId { get; private set; }
    public string? ErrorMessage { get; private set; }
    public IEnumerable<ErrorMessage> Errors { get; private set; } = Enumerable.Empty<ErrorMessage>();

    protected UseCaseResponse(UseCaseResponseKind status, T result)
    {
        Status = status;
        Result = result;
    }

    protected UseCaseResponse(UseCaseResponseKind status, T result, string resultId)
    {
        Status = status;
        Result = result;
        ResultId = resultId;
    }

    protected UseCaseResponse(UseCaseResponseKind status, string? errorMessage = null, IEnumerable<ErrorMessage>? errors = null)
    {
        ErrorMessage = errorMessage;
        Status = status;
        Errors = errors ?? Enumerable.Empty<ErrorMessage>();
    }

    public static UseCaseResponse<T> Persisted(T result, string resultId) => new(UseCaseResponseKind.DataPersisted, result, resultId);
    public static UseCaseResponse<T> Success(T result) => new(UseCaseResponseKind.Success, result);
    public static UseCaseResponse<T> Accepted(T result) => new(UseCaseResponseKind.DataAccepted, result);
    public static UseCaseResponse<T> Accepted(T result, string resultId) => new(UseCaseResponseKind.DataAccepted, result, resultId);
    public static UseCaseResponse<T> Unavailable(T result) => new(UseCaseResponseKind.Unavailable, result) { ErrorMessage = "Service Unavailable" };
    public static UseCaseResponse<T> NotFound() => NotFound(new ErrorMessage[] { new ErrorMessage(ErrorCodes.NotFound, ErrorMessages.NotFound) });
    public static UseCaseResponse<T> NotFound(IEnumerable<ErrorMessage> errors) => new(UseCaseResponseKind.NotFound, "Data not found", errors);
    public static UseCaseResponse<T> NotFound(string errorMessage)
    {
        var error = new ErrorMessage(ErrorCodes.NotFound, errorMessage);
        var errors = new[] { error };
        return new(UseCaseResponseKind.NotFound, errorMessage, errors);
    }
    public static UseCaseResponse<T> BadRequest(string errorMessage, IEnumerable<ErrorMessage> errors) => new(UseCaseResponseKind.BadRequest, errorMessage, errors);
    public static UseCaseResponse<T> BadRequest(string errorMessage)
    {
        var error = new ErrorMessage(ErrorCodes.BadRequest, errorMessage);
        var errors = new[] { error };
        return new(UseCaseResponseKind.BadRequest, errorMessage, errors);
    }
    public static UseCaseResponse<T> BadGateway() => BadGateway(new ErrorMessage[] { new ErrorMessage(ErrorCodes.BadGateway, ErrorMessages.UnhandledError) });
    public static UseCaseResponse<T> BadGateway(IEnumerable<ErrorMessage> errors) => new(UseCaseResponseKind.BadGateway, "Bad Gateway", errors);
    public static UseCaseResponse<T> InternalServerError(IEnumerable<ErrorMessage> errors) => new(UseCaseResponseKind.InternalServerError, "Internal Server Error", errors);
    public static UseCaseResponse<T> InternalServerError() => InternalServerError(new ErrorMessage[] { new ErrorMessage(ErrorCodes.UnhandledError, ErrorMessages.UnhandledError) });
    public static UseCaseResponse<T> InternalServerError(string errorMessage)
    {
        var error = new ErrorMessage(ErrorCodes.UnhandledError, errorMessage);
        var errors = new[] { error };
        return new(UseCaseResponseKind.InternalServerError, errorMessage, errors);
    }

    public bool Success() => string.IsNullOrEmpty(ErrorMessage) && !Errors.Any();
}
