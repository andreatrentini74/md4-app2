using Microsoft.Maui.Controls;
using QRBarcodeScannerApp.Services;
using QRBarcodeScannerApp.ViewModels;

namespace QRBarcodeScannerApp.Pages
{
    public partial class ConfigPage : ContentPage
    {
        public ConfigPage(AppSettings settings)
        {
            InitializeComponent();
            BindingContext = new ConfigViewModel(settings);
        }
    }
}