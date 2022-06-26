namespace Stronzo.PickToLight.Api.Shared.Constants;

public static class ErrorCodes
{
    private const string _prefix = "00.";

    public const string UnhandledError = $"{_prefix}01";
    public const string NotFound = $"{_prefix}02";
    public const string BadGateway = $"{_prefix}03";
    public const string BadRequest = $"{_prefix}04";
}
