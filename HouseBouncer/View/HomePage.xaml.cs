using HouseBouncer.ViewModels;
using Microsoft.Maui.Controls;
using HouseBouncer.Models;

namespace HouseBouncer.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage(HomeViewModel viewModel)
        {
            InitializeComponent();

            // Set the BindingContext
            BindingContext = viewModel;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRoom = e.CurrentSelection.FirstOrDefault() as Room;
            if (selectedRoom == null)
                return;

            var viewModel = BindingContext as HomeViewModel;
            viewModel?.RoomSelectedCommand.Execute(selectedRoom);

            // Deselect the item to allow re-selection
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
