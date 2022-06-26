using Stronzo.PickToLight.Api.Borders.Validators;

namespace Stronzo.PickToLight.Api.Configurations;

public static class ValidatorConfig
{
    public static IServiceCollection AddValidators(this IServiceCollection services) =>
        services.AddSingleton<AuthRequestValidator>()
                .AddSingleton<GetDevicesRequestValidator>()
                .AddSingleton<GetProductsRequestValidator>()
                ;
}
