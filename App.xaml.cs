using AcceptAFKServer.Presentation.Pages;

namespace AcceptAFKServer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Rout Registration 
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}