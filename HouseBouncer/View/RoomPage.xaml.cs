using HouseBouncer.Models;
using HouseBouncer.ViewModels;
//using Xamarin.Forms;

namespace HouseBouncer.Views
{
    public partial class RoomPage : ContentPage
    {
        RoomViewModel viewModel;

        public RoomPage()
        {
            InitializeComponent();
            viewModel = BindingContext as RoomViewModel;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDevice = e.CurrentSelection.FirstOrDefault() as DeviceModel;
            if (selectedDevice == null)
                return;

            viewModel.DeviceSelectedCommand.Execute(selectedDevice);

            // Deselect item
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
