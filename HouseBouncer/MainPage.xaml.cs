using Microsoft.Maui.Controls;

namespace SmartHomeApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Redirect to Shell
            Application.Current.MainPage = new AppShell();
        }
    }
}
