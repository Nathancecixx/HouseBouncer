using Microsoft.Maui.Controls;
using HouseBouncer.Views;

namespace HouseBouncer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Register routes for Room and Device pages
            Routing.RegisterRoute("room", typeof(RoomPage));
            Routing.RegisterRoute("device", typeof(DevicePage));
        }
    }
}
