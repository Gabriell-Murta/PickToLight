using Stronzo.PickToLight.Api.Borders.Repositories;
using Stronzo.PickToLight.Api.Borders.Repositories.Helpers;
using Stronzo.PickToLight.Api.Repositories;
using Stronzo.PickToLight.Api.Repositories.Helpers;

namespace Stronzo.PickToLight.Api.Configurations;

public static class RepositoryConfig
{
    public static IServiceCollection AddRepositories(this IServiceCollection services) =>
        services.AddSingleton<IRepositoryHelper, RepositoryHelper>()
                .AddSingleton<IUserRepository, UserRepository>()
                .AddSingleton<IDeviceRepository, DeviceRepository>()
                .AddSingleton<IProductRepository, ProductRepository>()
                ;
}
