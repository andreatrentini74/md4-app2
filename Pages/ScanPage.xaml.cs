using md4.ViewModels;

namespace md4.Pages;

public partial class ScanPage : ContentPage
{
	public ScanPage()
	{
		InitializeComponent();
		BindingContext = new ScanViewModel();
    }
}