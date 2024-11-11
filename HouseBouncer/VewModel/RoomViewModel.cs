using HouseBouncer.Models;
using HouseBouncer.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
//using Xamarin.Forms;

namespace HouseBouncer.ViewModels
{
    [QueryProperty(nameof(RoomId), "roomId")]
    public class RoomViewModel : BaseViewModel
    {
        private readonly DataService _dataService;

        private string roomId;
        public string RoomId
        {
            get => roomId;
            set
            {
                roomId = value;
                LoadRoomDevices();
            }
        }

        public ObservableCollection<DeviceModel> Devices { get; }

        public ICommand DeviceSelectedCommand { get; }

        public RoomViewModel()
        {
            Title = "Room";
            _dataService = new DataService();
            Devices = new ObservableCollection<DeviceModel>();
            DeviceSelectedCommand = new Command<DeviceModel>(OnDeviceSelected);
        }

        private void LoadRoomDevices()
        {
            IsBusy = true;

            var rooms = _dataService.GetRooms();
            var room = rooms.FirstOrDefault(r => r.Id == RoomId);
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

        private async void OnDeviceSelected(DeviceModel selectedDevice)
        {
            if (selectedDevice == null)
                return;

            // Navigate to DevicePage with the selected device's ID
            await Shell.Current.GoToAsync($"device?deviceId={selectedDevice.Id}");
        }
    }
}
