using md4.Services;
using md4.ViewModels;

namespace md4.Pages;

public partial class MainPage : ContentPage
{
	public MainPage(AppSettings settings)
	{
		InitializeComponent();
        BindingContext = new MainViewModel(settings);
    }
}