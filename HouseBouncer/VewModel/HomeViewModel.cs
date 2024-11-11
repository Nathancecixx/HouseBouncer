using SmartHomeApp.Models;
using SmartHomeApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
//using Xamarin.Forms;

namespace SmartHomeApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly DeviceManager _dataService;

        public ObservableCollection<Room> Rooms { get; }

        public ICommand RoomSelectedCommand { get; }

        public HomeViewModel()
        {
            Title = "Home";
            _dataService = new DeviceManager();
            Rooms = _dataService.GetRooms();
            RoomSelectedCommand = new Command<Room>(OnRoomSelected);
        }

        private async void OnRoomSelected(Room selectedRoom)
        {
            if (selectedRoom == null)
                return;

            // Navigate to RoomPage with the selected room's ID
            await Shell.Current.GoToAsync($"room?roomId={selectedRoom.Id}");
        }
    }
}
