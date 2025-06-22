using Microsoft.UI.Xaml.Controls;

namespace AcceptAFKServer.Pages;

public sealed partial class WifiPage : Page
{
    public WifiPage()
    {
        InitializeComponent();
    }




}


public record Wifi(string Name);