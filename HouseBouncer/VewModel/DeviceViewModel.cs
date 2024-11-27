using HouseBouncer.Models;
using HouseBouncer.Services;
using System.Linq;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Collections.ObjectModel;

namespace HouseBouncer.ViewModels
{
    [QueryProperty(nameof(DeviceId), "deviceId")]
    public class DeviceViewModel : BaseViewModel
    {
        private readonly DataService _dataService;

        private int deviceId;
        public int DeviceId
        {
            get => deviceId;
            set
            {
                deviceId = value;
                LoadDevice();
            }
        }

        public ObservableCollection<string> Resolutions { get; }
        public ObservableCollection<string> FanModes { get; }
        public ObservableCollection<string> CoolingModes { get; }

        private DeviceModel device;
        public DeviceModel Device
        {
            get => device;
            set => SetProperty(ref device, value);
        }

        public ICommand TogglePowerCommand { get; }
        public ICommand ToggleConnectionCommand { get; }

        public DeviceViewModel(DataService dataService)
        {
            Title = "Device";
            _dataService = dataService;

            TogglePowerCommand = new Command(OnTogglePower);
            ToggleConnectionCommand = new Command(OnToggleConnection);

            Resolutions = new ObservableCollection<string>
            {
                "1080p",
                "720p",
                "480p"
            };

            FanModes = new ObservableCollection<string>
            {
                "Normal",
                "Sleep",
                "Turbo"
            };

            CoolingModes = new ObservableCollection<string>
            {
                "Eco",
                "Normal",
                "Freeze"
            };
        }

        private void LoadDevice()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var rooms = _dataService.Rooms;
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

                if (Device == null)
                {
                    Title = "Device Not Found";
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnTogglePower()
        {
            if (Device != null)
            {
                Device.PowerStatus = !Device.PowerStatus;
            }
        }

        private void OnToggleConnection()
        {
            if (Device != null)
            {
                Device.IsConnected = !Device.IsConnected;
            }
        }
    }
}
