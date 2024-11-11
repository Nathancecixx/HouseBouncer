using SmartHomeApp.ViewModels;
//using Xamarin.Forms;

namespace SmartHomeApp.Views
{
    public partial class DevicePage : ContentPage
    {
        DeviceViewModel viewModel;

        public DevicePage()
        {
            InitializeComponent();
            viewModel = BindingContext as DeviceViewModel;
        }
    }
}
