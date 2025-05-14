using QRBarcodeScannerApp.ViewModels;

namespace QRBarcodeScannerApp.Pages;

public partial class ScanPage : ContentPage
{
	public ScanPage()
	{
		InitializeComponent();
		BindingContext = new ScanViewModel();
    }
}