using Microsoft.Maui.Controls;
using md4.Pages;

namespace md4
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