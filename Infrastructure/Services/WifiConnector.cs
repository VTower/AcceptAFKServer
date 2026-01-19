using Serilog;
using ManagedNativeWifi;
using System.Diagnostics;
using System.Net.NetworkInformation;
using AcceptAFKServer.Infrastructure.Services.Interfaces;
using AcceptAFKServer.Infrastructure.Common;

namespace AcceptAFKServer.Infrastructure.Services;

public class WifiConnector : IWifiConnector
{
    public List<string> _networks = [];
    public bool isEthernet = false;
    public bool isWifi = false;
    public WifiConnector()
    {
        Log.Information("#-# #-# #-# #-# WifiConnector #-# #-# #-# #-# ");
    }

    public async Task FindWifi()
    {
        Log.Information("#-# Find Wifi Start");

        // ! Isso foi mudado, antes setava _networks com atribuicao
        await GetWifiNetworks();
    }

    async Task<List<string>> GetWifiNetworks()
    {
        List<string> networks = [];

        // SO: Windows (10.0)
        if (OSHelper.IsWindows)
            await GetWifiWindows();

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

    async Task GetWifiWindows()
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


        foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
        {
            if (ni.OperationalStatus == OperationalStatus.Up)
            {
                Console.WriteLine($"Nome: {ni.Name}");
                Console.WriteLine($"Tipo: {ni.NetworkInterfaceType}");
                Console.WriteLine($"Descrição: {ni.Description}");
                Console.WriteLine($"ID: {ni.Id}");
                Console.WriteLine("------");
            }

            _networks.Add(ni.Name);
        }








        // ? NO WINDOWS NMCLI EH EQUIVALENTE A NETSH MAS EH MAIS FACIL PEGAR PELO POWER SHELL
        // Get-NetAdapter
        // Get-NetConnectionProfile 
        //     ProcessStartInfo procesInfo = new("nmcli", "-f SSID dev wifi")
        //     {
        //         RedirectStandardOutput = true,
        //         UseShellExecute = false,
        //         CreateNoWindow = true
        //     };

        //     using Process? process = Process.Start(procesInfo);

        //     using StreamReader? reader = process!.StandardOutput;

        //     while (!reader.EndOfStream)
        //     {
        //         string? line = reader.ReadLine();
        //         // TODO: Ver se precisa de tratativa para line null
        //         if (line is not null)
        //             _networks.Add(line.Trim());
        //     }

    }
}