using SmartHomeApp.Views;
//using Xamarin.Forms;

namespace SmartHomeApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for navigation
            Routing.RegisterRoute("room", typeof(RoomPage));
            Routing.RegisterRoute("device", typeof(DevicePage));
        }
    }
}
