using AcceptAFKServer.Infrastructure.Services;
using AcceptAFKServer.Infrastructure.Services.Interfaces;

namespace AcceptAFKServer.Infrastructure.Configuration;
public static class DependencySetup
{
    public static IServiceCollection AddWifiConnectorService(this IServiceCollection services)
    {
        services.AddSingleton<IWifiConnector, WifiConnector>();

        return services;
    }

}