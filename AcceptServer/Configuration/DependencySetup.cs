using AcceptServer.Services;
using AcceptServer.Services.Interfaces;

namespace AcceptServer.Configuration;
public static class DependencySetup
{
    public static IServiceCollection AddWifiConnectorService(this IServiceCollection services)
    {
        services.AddSingleton<IWifiConnector, WifiConnector>();

        return services;
    }

}