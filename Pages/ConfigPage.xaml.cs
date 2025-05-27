using Microsoft.Maui.Controls;
using md4.Services;
using md4.ViewModels;

namespace md4.Pages
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