using HouseBouncer.Models;
using HouseBouncer.Services;
using System;
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
        public ICommand DeleteRoomCommand { get; }

        public HomeViewModel(DataService dataService, IDialogService dialogService)
        {
            Title = "Home";
            _dataService = dataService;
            _dialogService = dialogService;
            Rooms = _dataService.Rooms;

            RoomSelectedCommand = new Command<Room>(OnRoomSelected);
            AddRoomCommand = new Command(OnAddRoom);
            DeleteRoomCommand = new Command<Room>(OnDeleteRoom);
        }

        private async void OnAddRoom()
        {
            // Prompt the user for the new room name
            string roomName = await _dialogService.ShowInputDialogAsync(
                "New Room",
                "Enter the name of the new room:",
                "Room Name"
            );

            if (string.IsNullOrWhiteSpace(roomName))
            {
                await _dialogService.ShowAlertAsync("Invalid Name", "Room name cannot be empty.", "OK");
                return;
            }

            // Generate a unique ID for the new room
            string newRoomId = Guid.NewGuid().ToString();

            Room newRoom = new Room
            {
                Id = newRoomId,
                Name = roomName,
                Devices = new ObservableCollection<DeviceModel>()
            };

            await _dataService.AddRoomAsync(newRoom);
        }

        private async void OnDeleteRoom(Room room)
        {
            if (room == null)
                return;

            // Ask the user to confirm the deletion
            bool confirm = await _dialogService.ShowConfirmationDialogAsync(
                "Delete Room",
                $"Are you sure you want to delete '{room.Name}'?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                _dataService.RemoveRoom(room);
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
