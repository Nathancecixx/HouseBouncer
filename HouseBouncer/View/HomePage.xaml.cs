using HouseBouncer.Models;
using HouseBouncer.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace HouseBouncer.Views
{
    public partial class HomePage : ContentPage
    {
        HomeViewModel viewModel;

        public HomePage()
        {
            InitializeComponent();

            // Resolve the HomeViewModel from DI
            viewModel = (Application.Current as App).Services.GetService<HomeViewModel>();

            // Set the BindingContext
            BindingContext = viewModel;
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
