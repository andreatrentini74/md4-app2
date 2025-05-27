using md4.ViewModels;
using System.Reflection;

namespace md4.Views;

public partial class CustomToolbar : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(CustomToolbar), default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Version
    {
        get
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return $"{version?.Major}.{version?.Minor}.{version?.Build}";

        }
    }

    public CustomToolbar()
    {
        InitializeComponent();
    }
}