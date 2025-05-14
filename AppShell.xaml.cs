using Microsoft.Maui.Controls;
using QRBarcodeScannerApp.Pages;

namespace QRBarcodeScannerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(ConfigPage), typeof(ConfigPage));
        }

    }
}