using Serilog;
using ManagedNativeWifi;
using System.Diagnostics;
using System.Net.NetworkInformation;
using AcceptAFKServer.Application.Common;
using AcceptAFKServer.Infrastructure.Services.Interfaces;

namespace AcceptAFKServer.Infrastructure.Services;

public class WifiConnector : IWifiConnector
{
    List<string> _networks = [];
    public bool isEthernet = false;
    public bool isWifi = false;
    public WifiConnector()
    {
        Log.Information("#-# #-# #-# #-# WifiConnector #-# #-# #-# #-# ");
    }

    public async Task FindWifi()
    {
        Log.Information("#-# Find Wifi Start");

        _networks = await GetWifiNetworks();

        await Task.Delay(1000);
    }

    async Task<List<string>> GetWifiNetworks()
    {
        List<string> networks = [];

        // SO: Windows (10.0)
        if (OSHelper.IsWindows)
        {
            await Task.Run(() =>
            {
                NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                foreach (var networkInterface in networkInterfaces)
                {
                    // Ignorar interfaces que não estão conectadas
                    if (networkInterface.OperationalStatus != OperationalStatus.Up)
                        continue;

                    // Tipo de interface: Wi-Fi, Ethernet, etc.
                    NetworkInterfaceType interfaceType = networkInterface.NetworkInterfaceType;

                    // Todo: Configura se é ou não WiFI 
                    isWifi = interfaceType == NetworkInterfaceType.Wireless80211;
                    isEthernet = interfaceType == NetworkInterfaceType.Ethernet;

                    Log.Information($"Interface: {networkInterface.Name}");
                    Log.Information($"Descrição: {networkInterface.Description}");
                    Log.Information($"Tipo: {(isWifi ? "Wi-Fi" : isEthernet ? "Cabo" : "Outro")}");

                    // Obtém informações do endereço IP associado
                    var ipProps = networkInterface.GetIPProperties();
                    var gateway = ipProps.GatewayAddresses.FirstOrDefault()?.Address;

                    Log.Information($"Gateway: {gateway}");
                    Log.Information(new string('-', 30));

                    // Obtém o nome da rede (SSID para Wi-Fi)
                    if (isWifi)
                    {
                        var connectedNetwork = NativeWifi.EnumerateAvailableNetworks();
                        Log.Information($"SSID conectado: {connectedNetwork}");
                    }
                }

                ProcessStartInfo procesInfo = new("nmcli", "-f SSID dev wifi")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process? process = Process.Start(procesInfo);

                using StreamReader? reader = process!.StandardOutput;

                while (!reader.EndOfStream)
                {
                    string? line = reader.ReadLine();
                    // TODO: Ver se precisa de tratativa para line null
                    if (line is not null)
                        networks.Add(line.Trim());
                }
            });
        }

        // SO: Linux (Suse)
        if (OSHelper.IsLinux)
        {
            await Task.Run(() =>
        {
            ProcessStartInfo procesInfo = new("netsh", "wlan show network")
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = Process.Start(procesInfo);
            using var reader = process!.StandardOutput;
            while (!reader.EndOfStream)
            {
                string? line = reader.ReadLine();
                // TODO: Ver se precisa de tratativa para line null
                if (line is not null)
                    if (line.Contains("SSID"))
                        networks.Add(line.Trim());
            }
        });

            //Remove command format table name
            networks.Remove(networks[0]);

        }

        return networks;
    }
}