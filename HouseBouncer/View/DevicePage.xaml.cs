using HouseBouncer.ViewModels;

namespace HouseBouncer.Views
{
    public partial class DevicePage : ContentPage
    {

        public DevicePage(DeviceViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
