using System.Collections.ObjectModel;
using AcceptAFKServer.Infrastructure.Services;

namespace AcceptAFKServer.Presentation.Pages;

public partial class ConfigurationPage : ContentView
{
	public ObservableCollection<string>? WifiList { get; set; } = [];
	public string? SelectedWifi { get; set; }
	private CancellationTokenSource _rotationToken;

	public ConfigurationPage()
	{
		InitializeComponent();
		BindingContext = this;
		_rotationToken = new CancellationTokenSource();
	}

	private async void OnReloadWifiClicked(object sender, EventArgs e)
	{
		StartRotation(_rotationToken.Token);

		await LoadWifis();

		_rotationToken.Cancel();
		RefreshButton.Rotation = 0;
	}

	private async Task AnimateRefreshAsync()
	{
		RefreshButton.IsEnabled = false;

		await RefreshButton.RotateToAsync(360, 600, Easing.CubicInOut)!;
		RefreshButton.Rotation = 0;

		RefreshButton.IsEnabled = true;
	}

	private async void StartRotation(CancellationToken token)
	{
		try
		{
			while (!token.IsCancellationRequested)
			{
				await RefreshButton.RotateToAsync(360, 800, Easing.Linear);
				RefreshButton.Rotation = 0;
			}
		}
		catch { }
	}

	private async Task LoadWifis()
	{
		WifiList?.Clear();

		WifiConnector wifiConnector = new();

		await wifiConnector.FindWifi();

		foreach (string wifi in wifiConnector._networks)
			WifiList?.Add(wifi);
	}
}