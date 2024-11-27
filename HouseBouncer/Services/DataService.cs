using HouseBouncer.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace HouseBouncer.Services
{
    public class DataService
    {
        private const string DataFileName = "data.json";
        private ObservableCollection<Room> rooms;

        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            private set => rooms = value;
        }

        private readonly JsonSerializerOptions _jsonOptions;

        public DataService()
        {
            Rooms = new ObservableCollection<Room>();
            _jsonOptions = new JsonSerializerOptions
            {
                // This ensures that the polymorphic attributes are respected
                WriteIndented = true,
            };
            Rooms.CollectionChanged += OnRoomsCollectionChanged;
            // Initialize data from storage asynchronously
            LoadDataAsync().ConfigureAwait(false);
        }

        public async Task LoadDataAsync()
        {
            var filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                var loadedRooms = JsonSerializer.Deserialize<ObservableCollection<Room>>(json, _jsonOptions);
                if (loadedRooms != null)
                {
                    // Update the Rooms collection in-place
                    Rooms.Clear();
                    foreach (var room in loadedRooms)
                    {
                        Rooms.Add(room);
                        // Subscribe to collection changes in devices
                        room.Devices.CollectionChanged += OnDevicesCollectionChanged;
                        // Subscribe to property changes in devices
                        foreach (var device in room.Devices)
                        {
                            SubscribeToDeviceChanges(device);
                        }
                    }
                }
            }
            else
            {
                // Initialize with placeholder data if no file exists
                var defaultRooms = GetDefaultRooms();
                Rooms.Clear();
                foreach (var room in defaultRooms)
                {
                    Rooms.Add(room);
                    // Subscribe to collection changes in devices
                    room.Devices.CollectionChanged += OnDevicesCollectionChanged;
                    // Subscribe to property changes in devices
                    foreach (var device in room.Devices)
                    {
                        SubscribeToDeviceChanges(device);
                    }
                }
                await SaveDataAsync();
            }
        }


        public async Task SaveDataAsync()
        {
            try
            {
                if (Rooms.Count == 0)
                {
                    Console.WriteLine("No data to save.");
                }

                string json = JsonSerializer.Serialize(Rooms, _jsonOptions);
                Console.WriteLine($"Saving JSON: {json}");
                await File.WriteAllTextAsync(GetFilePath(), json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving data: {ex.Message}");
            }
        }


        private string GetFilePath()
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Console.WriteLine(folderPath);
            return Path.Combine(folderPath, DataFileName);
        }

        private ObservableCollection<Room> GetDefaultRooms()
        {
            return new ObservableCollection<Room>
            {
                new Room
                {
                    Id = "1",
                    Name = "Living Room",
                    Devices = new ObservableCollection<DeviceModel>
                    {
                    }
                },
                new Room
                {
                    Id = "2",
                    Name = "Bedroom",
                    Devices = new ObservableCollection<DeviceModel>
                    {
                    }
                }
            };
        }

        public void UpdateDevice(DeviceModel updatedDevice)
        {
            // Find the room containing the device
            var room = Rooms.FirstOrDefault(r => r.Id == updatedDevice.RoomId);
            if (room != null)
            {
                // Find the device in the room and update it
                var device = room.Devices.FirstOrDefault(d => d.Id == updatedDevice.Id);
                if (device != null)
                {
                    // Update the device properties
                    device.Name = updatedDevice.Name;
                    device.Type = updatedDevice.Type;
                    device.PowerStatus = updatedDevice.PowerStatus;
                    device.IsConnected = updatedDevice.IsConnected;

                    if (device is Camera camera && updatedDevice is Camera updatedCamera)
                    {
                        camera.IsRecording = updatedCamera.IsRecording;
                        camera.Resolution = updatedCamera.Resolution;
                        camera.Angle = updatedCamera.Angle;
                    }
                    else if (device is GarageDoor garageDoor && updatedDevice is GarageDoor updatedGarageDoor)
                    {
                        garageDoor.Status = updatedGarageDoor.Status;
                        garageDoor.IsLocked = updatedGarageDoor.IsLocked;
                        garageDoor.LastOpened = updatedGarageDoor.LastOpened;
                    }
                }
            }
        }


        public void AddRoom(Room newRoom)
        {
            Rooms.Add(newRoom);
        }

        public void RemoveRoom(Room room)
        {
            Rooms.Remove(room);
        }


        private void OnRoomsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Room room in e.NewItems)
                {
                    room.Devices.CollectionChanged += OnDevicesCollectionChanged;
                    foreach (var device in room.Devices)
                    {
                        SubscribeToDeviceChanges(device);
                    }
                }
            }

            if (e.OldItems != null)
            {
                foreach (Room room in e.OldItems)
                {
                    room.Devices.CollectionChanged -= OnDevicesCollectionChanged;
                }
            }
        }

        private void OnDevicesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (DeviceModel device in e.NewItems)
                {
                    SubscribeToDeviceChanges(device);
                }
            }

            if (e.OldItems != null)
            {
                foreach (DeviceModel device in e.OldItems)
                {
                    device.PropertyChanged -= OnDevicePropertyChanged;
                }
            }
        }

        private void SubscribeToDeviceChanges(DeviceModel device)
        {
            device.PropertyChanged += OnDevicePropertyChanged;
        }

        private async void OnDevicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is DeviceModel updatedDevice)
            {
                UpdateDevice(updatedDevice);
                await SaveDataAsync();
            }
        }
    }
}
