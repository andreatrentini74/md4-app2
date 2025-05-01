using QRBarcodeScannerApp.Services;
using QRBarcodeScannerApp.ViewModels;

namespace QRBarcodeScannerApp.Pages;

public partial class ScannerPage : ContentPage
{
	public ScannerPage(AppSettings settings)
	{
		InitializeComponent();
        BindingContext = new ScannerViewModel(settings);
    }
}