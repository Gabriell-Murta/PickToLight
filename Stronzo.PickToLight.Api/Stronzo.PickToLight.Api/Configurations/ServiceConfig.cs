using Stronzo.PickToLight.Api.Borders.Services;
using Stronzo.PickToLight.Api.UseCases.Services;

namespace Stronzo.PickToLight.Api.Configurations
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services) =>
            services.AddSingleton<IAuthService, AuthService>()
                    ;
    }
}
