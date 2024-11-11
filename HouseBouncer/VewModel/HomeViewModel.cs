using HouseBouncer.Models;
using HouseBouncer.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace HouseBouncer.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Room> Rooms { get; }

        public ICommand RoomSelectedCommand { get; }
        public ICommand AddRoomCommand { get; }

        public HomeViewModel(DataService dataService, IDialogService dialogService)
        {
            Title = "Home";
            _dataService = dataService;
            _dialogService = dialogService;
            Rooms = _dataService.GetRooms();
            RoomSelectedCommand = new Command<Room>(OnRoomSelected);
            AddRoomCommand = new Command(OnAddRoom);
        }

        private async void OnAddRoom()
        {
            string roomName = await _dialogService.ShowInputDialogAsync("New Room", "Enter the name of the new room:", "Room Name");

            if (!string.IsNullOrWhiteSpace(roomName))
            {
                // Generate a unique ID for the new room
                string newRoomId = Guid.NewGuid().ToString();

                // Create a new Room object
                Room newRoom = new Room
                {
                    Id = newRoomId,
                    Name = roomName,
                    Devices = new ObservableCollection<DeviceModel>() // Initialize with no devices
                };

                // Add the new room to the DataService
                _dataService.AddRoom(newRoom);
            }
            else
            {
                await _dialogService.ShowAlertAsync("Invalid Name", "Room name cannot be empty.", "OK");
            }
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
