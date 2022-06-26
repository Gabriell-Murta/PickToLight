namespace Stronzo.PickToLight.Api.Borders.Shared;

public enum UseCaseResponseKind
{
    Success,
    OK,
    DataPersisted,
    DataAccepted,
    InternalServerError,
    RequestValidationError,
    ForeignKeyViolationError,
    UniqueViolationError,
    NotFound,
    Unauthorized,
    Forbidden,
    BadRequest,
    BadGateway,
    Unavailable,
    UnprocessableEntity
}
