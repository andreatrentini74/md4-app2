using Microsoft.Maui.Controls;
using QRBarcodeScannerApp.Pages;

namespace QRBarcodeScannerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));
            Routing.RegisterRoute(nameof(ConfigPage), typeof(ConfigPage));
        }

    }
}