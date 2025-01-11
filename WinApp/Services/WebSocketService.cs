using System;
using System.Text;
using System.Threading;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace AcceptAFKServer.Services;

public class WebSocketService : IHostedService
{
    // Use this to case need refresh connection
    private bool _firstTime = false;
    private readonly ClientWebSocket _webSocket;
    private readonly HttpListener _httpListener;
    private string LocalPortURL => "http://localhost:5055/";
    private readonly CancellationTokenSource _cancellationTokenSource;

    public WebSocketService()
    {
        _webSocket = new ClientWebSocket();
        _httpListener = new HttpListener();
        _httpListener.Prefixes.Add(LocalPortURL);
        _cancellationTokenSource = new CancellationTokenSource();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _httpListener.Start();
        Console.WriteLine("Server WS initiate. Waiting Connections...");

        _ = Task.Run(async () => await AcceptConnectionsAsync(), CancellationToken.None);
    }

    private async Task AcceptConnectionsAsync()
    {
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            try
            {
                HttpListenerContext context = await _httpListener.GetContextAsync();

                if (context.Request.IsWebSocketRequest)
                {
                    Console.WriteLine("Conexão WebSocket recebida.");
                    HttpListenerWebSocketContext webSocketContext = await context.AcceptWebSocketAsync(null);
                    _ = Task.Run(() => HandleClientAsync(webSocketContext.WebSocket));
                }
                else
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao aceitar conexão: {ex.Message}");
            }
        }
    }

    private async Task HandleClientAsync(WebSocket webSocket)
    {
        byte[] buffer = new byte[1024];

        try
        {
            while (webSocket.State == WebSocketState.Open)
            {
                //TODO: Atualizar Fluxo !
                // While (WebSocketState.Open)/
                // 1 - Enviar o Status de cituação do Jogo em numeros.
                // 2 - Sleep de 100ms 
                // 3 - Checar se recebe mensagem do cliente apos 5 ciclos (Testar se o Fluxo não para apartir daqui)
                // 4 - Voltar ao while

                // Receive Message
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("Cliente desconectado.");
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
                else
                {
                    string? message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    Console.WriteLine($"Mensagem recebida: {message}");

                    byte[] response = Encoding.UTF8.GetBytes($"Servidor recebeu: {message}");

                    // Send Message
                    await webSocket.SendAsync(new ArraySegment<byte>(response), WebSocketMessageType.Text, true, _cancellationTokenSource.Token);
                }

                await Task.Delay(100);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro na conexão com o cliente: {ex.Message}");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cancellationTokenSource.Cancel();

        if (_webSocket.State == WebSocketState.Open)
        {
            return _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", cancellationToken);
        }

        return Task.CompletedTask;
    }

    //private async Task ReceiveMessagesAsync()
    //{
    //    byte[] buffer = new byte[1024];

    //    try
    //    {
    //        while (!_cancellationTokenSource.Token.IsCancellationRequested &&
    //               _webSocket.State == WebSocketState.Open)
    //        {
    //            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), _cancellationTokenSource.Token);

    //            if (result.MessageType == WebSocketMessageType.Close)
    //            {
    //                Console.WriteLine("Servidor solicitou fechamento do WebSocket.");
    //                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by server", CancellationToken.None);
    //            }
    //            else
    //            {
    //                string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Erro no WebSocket: {ex.Message}");
    //    }
    //}
}