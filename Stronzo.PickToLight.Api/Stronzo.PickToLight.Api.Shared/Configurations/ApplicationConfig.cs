namespace Stronzo.PickToLight.Api.Shared.Configurations;

public record ApplicationConfig
{
    public ConnectionStrings ConnectionStrings { get; init; }
    public AuthenticationConfiguration Authentication { get; init; }
}

public record ConnectionStrings
{
    public string DefaultConnection { get; init; }
}

public record AuthenticationConfiguration
{
    public string Secret { get; init; }
}
