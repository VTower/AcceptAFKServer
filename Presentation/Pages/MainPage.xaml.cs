namespace AcceptAFKServer.Presentation.Pages;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        Title = "AcceptServer";
    }

    private async void Start_Button(object sender, EventArgs e)
    {
        Window.Page = new HomePage();
    }
}