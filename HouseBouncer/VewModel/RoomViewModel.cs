using HouseBouncer.Models;
using HouseBouncer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace HouseBouncer.ViewModels
{
    [QueryProperty(nameof(RoomId), "roomId")]
    public class RoomViewModel : BaseViewModel
    {
        private readonly DataService _dataService;
        private readonly IDialogService _dialogService;

        private string roomId;
        public string RoomId
        {
            get => roomId;
            set
            {
                if (SetProperty(ref roomId, value))
                {
                    LoadRoomDevices();
                }
            }
        }

        public ObservableCollection<DeviceModel> Devices { get; }

        public ICommand DeviceSelectedCommand { get; }
        public ICommand AddDeviceCommand { get; }
        public ICommand DeleteDeviceCommand { get; }

        public RoomViewModel(DataService dataService, IDialogService dialogService)
        {
            Title = "Room";
            _dataService = dataService;
            _dialogService = dialogService;
            Devices = new ObservableCollection<DeviceModel>();
            DeviceSelectedCommand = new Command<DeviceModel>(OnDeviceSelected);
            AddDeviceCommand = new Command(OnAddDevice);
            DeleteDeviceCommand = new Command<DeviceModel>(OnDeleteDevice);
        }

        private void LoadRoomDevices()
        {
            IsBusy = true;

            var room = _dataService.Rooms.FirstOrDefault(r => r.Id == RoomId);
            if (room != null)
            {
                Title = room.Name;
                Devices.Clear();
                foreach (var device in room.Devices)
                {
                    Devices.Add(device);
                }
            }

            IsBusy = false;
        }

        private async void OnAddDevice()
        {
            // Prompt the user for the new device name
            string deviceName = await _dialogService.ShowInputDialogAsync(
                "Add New Device",
                "Enter the name of the new device:",
                "Device Name");

            if (string.IsNullOrWhiteSpace(deviceName))
                return;

            var newDevice = new DeviceModel
            {
                Id = _dataService.Rooms.SelectMany(r => r.Devices).Max(d => d.Id) + 1,
                Name = deviceName
            };

            // Find the room to which the device will be added
            var room = _dataService.Rooms.FirstOrDefault(r => r.Id == RoomId);
            if (room != null)
            {
                room.Devices.Add(newDevice);
                // For UI
                Devices.Add(newDevice);
                await _dataService.SaveDataAsync();
            }
            else
            {
                await _dialogService.ShowAlertAsync("Error", "Room not found.", "OK");
            }
        }

        private async void OnDeleteDevice(DeviceModel device)
        {
            if (device == null)
                return;

            bool confirm = await _dialogService.ShowConfirmationDialogAsync(
                "Delete Device",
                $"Are you sure you want to delete '{device.Name}'?",
                "Yes",
                "No"
            );

            if (confirm)
            {
                // Find the room
                var room = _dataService.Rooms.FirstOrDefault(r => r.Id == RoomId);
                if (room != null)
                {
                    room.Devices.Remove(device);
                    Devices.Remove(device);
                    await _dataService.SaveDataAsync();
                }
                else
                {
                    await _dialogService.ShowAlertAsync("Error", "Room not found.", "OK");
                }
            }
        }

        private async void OnDeviceSelected(DeviceModel selectedDevice)
        {
            if (selectedDevice == null)
                return;

            // Navigate to DevicePage with the selected device's ID
            await Shell.Current.GoToAsync($"device?deviceId={selectedDevice.Id}");
        }
    }
}
