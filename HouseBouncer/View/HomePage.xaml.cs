using SmartHomeApp.Models;
using SmartHomeApp.ViewModels;
//using Xamarin.Forms;

namespace SmartHomeApp.Views
{
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;

        public HomePage()
        {
            InitializeComponent();
            viewModel = BindingContext as HomeViewModel;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRoom = e.CurrentSelection.FirstOrDefault() as Room;
            if (selectedRoom == null)
                return;

            viewModel.RoomSelectedCommand.Execute(selectedRoom);

            // Deselect item
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}
