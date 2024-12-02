using System;
using Microsoft.UI.Xaml;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Controls;


namespace AcceptAFKServer.Pages;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        InitializeComponent();
        CreateFlipViewItems();

    }
    private void Return_StartPage(object sender, RoutedEventArgs e)
    {
        Frame.Navigate(typeof(StartPage));
    }

    private void NavigateBarButton(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("Botão clicado!");
        Button button = (Button)sender;

        switch (button.Name)
        {
            case "Lupa":
                if (ContentFrame.CurrentSourcePageType != typeof(GamesPage))
                    ContentFrame.Navigate(typeof(GamesPage));
                break;
            case "Config":
                if (ContentFrame.CurrentSourcePageType != typeof(ConfigurationPage))
                    ContentFrame.Navigate(typeof(ConfigurationPage));
                break;
            case "Wifi":
                if (ContentFrame.CurrentSourcePageType != typeof(WifiPage))
                    ContentFrame.Navigate(typeof(WifiPage));
                break;
        }
    }

    // TODO: Mudar nome e ver se vou manter 
    private void MyFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        FlipView flipView = (FlipView)sender;

        if (flipView == null || flipView.SelectedIndex == flipView.Items.Count || flipView.SelectedIndex == 0 || flipView.Items.Count == 0)
            return;
    }

    private void CreateFlipViewItems()
    {
        List<UIElement> icons =
        [
            //Lupa
            CreateButtonWithIcon("\uE721", "Lupa"),
            //Config
            CreateButtonWithIcon("\uE713", "Config"),
            //Wifi
            CreateButtonWithIcon("\uE701", "Wifi")
        ];

        MyFlipView.ItemsSource = icons;
    }

    private Button CreateButtonWithIcon(string glyph, string name = null)
    {
        Button button = new()
        {
            Name = name,
            Content = new FontIcon
            {
                Glyph = glyph,
                FontSize = 60
            },
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
        };

        button.Click += NavigateBarButton;

        return button;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // Ação a ser executada quando o botão for clicado
        Console.WriteLine("Botão clicado!");
    }
}
