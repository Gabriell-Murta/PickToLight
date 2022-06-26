using Stronzo.PickToLight.Api.Borders.UseCases.Auth;
using Stronzo.PickToLight.Api.Borders.UseCases.Device;
using Stronzo.PickToLight.Api.Borders.UseCases.Product;
using Stronzo.PickToLight.Api.Borders.UseCases.User;
using Stronzo.PickToLight.Api.UseCases.Auth;
using Stronzo.PickToLight.Api.UseCases.Device;
using Stronzo.PickToLight.Api.UseCases.Product;
using Stronzo.PickToLight.Api.UseCases.User;

namespace Stronzo.PickToLight.Api.Configurations;

public static class UseCaseConfig
{
    public static IServiceCollection AddUseCases(this IServiceCollection services) =>
        services.AddSingleton<IAuthenticationUseCase, AuthenticationUseCase>()
                .AddSingleton<IGetUserByIdUseCase, GetUserByIdUseCase>()
                .AddSingleton<IGetDevicesUseCase, GetDevicesUseCase>()
                .AddSingleton<IGetProductsUseCase, GetProductsUseCase>()
                ;
}
