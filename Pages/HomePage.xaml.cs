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
        ContentFrame.Navigate(typeof(ConfigurationPage));
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
            CreateButtonWithIcon("\uE721"),
            //Config
            CreateButtonWithIcon("\uE713"),
            //Wifi
            CreateButtonWithIcon("\uE701")
        ];

        MyFlipView.ItemsSource = icons;
    }

    private Button CreateButtonWithIcon(string glyph)
    {
        Button button = new()
        {
            Content = new FontIcon
            {
                Glyph = glyph,
                FontSize = 100
            },
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
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
