using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AcceptServer.Common;
using AcceptServer.Services.Interfaces;
using ManagedNativeWifi;

namespace AcceptServer.Services;
public class WifiConnector : IWifiConnector
{
    readonly ILogger<WifiConnector> _logger;
    List<string> _networks = [];
    public bool isEthernet = false;
    public bool isWifi = false;
    public WifiConnector(ILogger<WifiConnector> logger)
    {
        _logger = logger;
        _logger.LogInformation("#-# #-# #-# #-# WifiConnector #-# #-# #-# #-# ");
    }

    public async Task FindWifi()
    {
        _logger.LogInformation("#-# Find Wifi Start");

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

                    Console.WriteLine($"Interface: {networkInterface.Name}");
                    Console.WriteLine($"Descrição: {networkInterface.Description}");
                    Console.WriteLine($"Tipo: {(isWifi ? "Wi-Fi" : isEthernet ? "Cabo" : "Outro")}");

                    // Obtém informações do endereço IP associado
                    var ipProps = networkInterface.GetIPProperties();
                    var gateway = ipProps.GatewayAddresses.FirstOrDefault()?.Address;

                    Console.WriteLine($"Gateway: {gateway}");
                    Console.WriteLine(new string('-', 30));

                    // Obtém o nome da rede (SSID para Wi-Fi)
                    if (isWifi)
                    {
                      var connectedNetwork = NativeWifi.EnumerateAvailableNetworks();
                        Console.WriteLine($"SSID conectado: {connectedNetwork}");
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