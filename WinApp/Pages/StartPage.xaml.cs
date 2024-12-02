using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;


namespace AcceptAFKServer.Pages;

public sealed partial class StartPage : Page
{
    public StartPage()
    {
        InitializeComponent();
    }

    private void Start_Button(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(HomePage));
    }

}
