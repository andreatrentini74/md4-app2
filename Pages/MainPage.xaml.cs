using QRBarcodeScannerApp.Services;
using QRBarcodeScannerApp.ViewModels;

namespace QRBarcodeScannerApp.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(AppSettings settings)
	{
		InitializeComponent();
        BindingContext = new MainViewModel(settings);
    }
}