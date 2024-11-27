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

        private DeviceModel device;
        public DeviceModel Device
        {
            get => device;
            set => SetProperty(ref device, value);
        }

        public ICommand TogglePowerCommand { get; }
        public ICommand ToggleConnectionCommand { get; }

        // Constructor with DI
        public DeviceViewModel(DataService dataService)
        {
            Title = "Device";
            _dataService = dataService;

            // Initialize commands
            TogglePowerCommand = new Command(OnTogglePower);
            ToggleConnectionCommand = new Command(OnToggleConnection);

            Resolutions = new ObservableCollection<string>
            {
                "1080p",
                "720p",
                "480p"
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
                    // Handle device not found
                    Title = "Device Not Found";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (optional)
                Title = "Error";
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
                Device.powerStatus = !Device.powerStatus;
                // Optionally, save the state or perform additional actions
                OnPropertyChanged(nameof(Device));
            }
        }

        private void OnToggleConnection()
        {
            if (Device != null)
            {
                Device.isConnected = !Device.isConnected;
                // Optionally, save the state or perform additional actions
                OnPropertyChanged(nameof(Device));
            }
        }
    }
}
