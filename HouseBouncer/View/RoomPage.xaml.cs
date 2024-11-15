using HouseBouncer.Models;
using HouseBouncer.ViewModels;

namespace HouseBouncer.Views
{
    public partial class RoomPage : ContentPage
    {

        public RoomPage(RoomViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDevice = e.CurrentSelection.FirstOrDefault() as DeviceModel;
            if (selectedDevice == null)
                return;

            var viewModel = BindingContext as RoomViewModel;
            viewModel?.DeviceSelectedCommand.Execute(selectedDevice);

            // Deselect item
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
