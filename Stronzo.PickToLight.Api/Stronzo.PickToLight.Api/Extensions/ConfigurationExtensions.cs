using Stronzo.PickToLight.Api.Shared.Configurations;

namespace Stronzo.PickToLight.Api.Extensions;

public static class ConfigurationExtensions
{
    public static ApplicationConfig LoadConfiguration(this IConfiguration source)
    {
        var applicationConfig = source.Get<ApplicationConfig>();

        return applicationConfig;
    }
}
