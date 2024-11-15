using HouseBouncer.ViewModels;
//using Xamarin.Forms;

namespace HouseBouncer.Views
{
    public partial class DevicePage : ContentPage
    {
        DeviceViewModel viewModel;

        public DevicePage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
