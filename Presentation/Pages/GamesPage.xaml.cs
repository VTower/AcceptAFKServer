namespace AcceptAFKServer.Presentation.Pages;

public partial class GamesPage : ContentView
{
	public GamesPage()
	{
		InitializeComponent();
		BindingContext = this;
	}
	public List<string> Games { get; } =
	[
		"counter_strike_logo.png",
		"dota_logo.png",
		"league_of_legends_logo.png",
	];
}