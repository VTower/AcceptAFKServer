using System;
using Microsoft.UI.Xaml;
using AcceptAFKServer.Pages;
using AcceptAFKServer.Common;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace AcceptAFKServer;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    private Window m_window;
    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
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
}
