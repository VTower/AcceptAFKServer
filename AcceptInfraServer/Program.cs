using AcceptServer.Aplication;
using AcceptServer.Configuration;
using AcceptServer.Services.Interfaces;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddHostedService<Worker>()
    .AddWifiConnectorService();

IHost host = builder.Build();

IWifiConnector instanceWIFI = host.Services.GetRequiredService<IWifiConnector>();
await instanceWIFI.FindWifi();

host.Run();
