using System;
using Microsoft.UI.Xaml;
using AcceptAFKServer.Pages;
using AcceptAFKServer.Common;
using AcceptAFKServer.Services;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AcceptAFKServer;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window m_window;
    public IHost Host
    {
        get;
    }

    public App()
    {
        InitializeComponent();

        Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Register WebSocketService
                services.AddSingleton<WebSocketService>();
            })
            .Build();

        Host.Start();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        m_window = new MainWindow();

        // Modifica e centraliza a tela.
        WindowHandlerHelper.ResizeAndCenterWindow(m_window, 650, 900);

        // Create a Frame to act as the navigation context and navigate to the first page
        Frame rootFrame = new();
        rootFrame.NavigationFailed += OnNavigationFailed;
        // Navigate to the first page, configuring the new page
        // by passing required information as a navigation parameter
        rootFrame.Navigate(typeof(StartPage), args.Arguments);

        // Place the frame in the current Window
        m_window.Content = rootFrame;
        // Ensure the MainWindow is active
        m_window.Activate();
    }

    private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    // Used this to capture the instance of IHost
    public static T GetService<T>() where T : class
    {
        return ((App)Current).Host.Services.GetService(typeof(T)) as T;
    }

    // Used this to capture the instance of WebSocketService
    public static WebSocketService GetWebSocketServiceService()
    {
        return ((App)Current).Host.Services.GetService(typeof(WebSocketService)) as WebSocketService;
    }
}
