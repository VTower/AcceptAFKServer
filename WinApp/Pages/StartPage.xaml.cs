using Microsoft.UI.Xaml;
using AcceptAFKServer.Services;
using Microsoft.UI.Xaml.Controls;


namespace AcceptAFKServer.Pages;

public sealed partial class StartPage : Page
{
    readonly WebSocketService _webSocketService;

    public StartPage()
    {
        InitializeComponent();

        //TODO: Adicionar a tela de acionamento de jogo e passar o joogo como parametro
        // Obter o serviço do Host
        _webSocketService = App.GetService<WebSocketService>();
    }

    private async void Start_Button(object sender, RoutedEventArgs e)
    {
        //TODO: Adicionar a tela de acionamento de jogo e passar o joogo como parametro
        await _webSocketService.StartAsync(new());

        Frame.Navigate(typeof(HomePage));
    }

}
