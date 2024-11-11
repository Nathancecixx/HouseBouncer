using SmartHomeApp.Models;
using SmartHomeApp.Services;
using System.Linq;

namespace SmartHomeApp.ViewModels
{
    [QueryProperty(nameof(DeviceId), "deviceId")]
    public class DeviceViewModel : BaseViewModel
    {
        private readonly DeviceManager _dataService;

        private string deviceId;
        public string DeviceId
        {
            get => deviceId;
            set
            {
                deviceId = value;
                LoadDevice();
            }
        }

        private DeviceModel device;
        public DeviceModel Device
        {
            get => device;
            set => SetProperty(ref device, value);
        }

        public DeviceViewModel()
        {
            Title = "Device";
            _dataService = new DeviceManager();
        }

        private void LoadDevice()
        {
            IsBusy = true;

            var rooms = _dataService.GetRooms();
            foreach (var room in rooms)
            {
                var foundDevice = room.Devices.FirstOrDefault(d => d.Id == DeviceId);
                if (foundDevice != null)
                {
                    Device = foundDevice;
                    Title = Device.Name;
                    break;
                }
            }

            IsBusy = false;
        }

        // Add commands and properties to manage the device
    }
}
