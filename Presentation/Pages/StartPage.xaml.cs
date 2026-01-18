
using AcceptAFKServer.Infrastructure.Services;

namespace AcceptAFKServer.Presentation.Pages;

public partial class StartPage : ContentPage
{
    readonly WebSocketService? _webSocketService;

    public StartPage()
    {
        InitializeComponent();
        //TODO: Adicionar a tela de acionamento de jogo e passar o joogo como parametro
        // Obter o servi�o do Host
        // _webSocketService = App.GetService<WebSocketService>();
    }

    // private async void Start_Button(object sender, RoutedEventArgs e)
    // {
    //     //TODO: Adicionar a tela de acionamento de jogo e passar o joogo como parametro
    //     await _webSocketService.StartAsync(new());

    //     Frame.Navigate(typeof(HomePage));
    // }
}