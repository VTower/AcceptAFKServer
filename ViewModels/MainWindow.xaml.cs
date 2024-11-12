using Microsoft.UI.Xaml;

namespace AcceptAFKServer
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void StartButton(object sender, RoutedEventArgs e)
        {
            start_Button.Content = "Clicked";
        }
    }
}
