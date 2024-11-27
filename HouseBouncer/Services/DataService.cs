using HouseBouncer.Models;
using System.Collections.ObjectModel;
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
            var room = Rooms.FirstOrDefault(r => r.Id == updatedDevice.roomId);
            if (room != null)
            {
                // Find the device in the room and update it
                var device = room.Devices.FirstOrDefault(d => d.Id == updatedDevice.Id);
                if (device != null)
                {
                    // Update the device properties
                    device.Name = updatedDevice.Name;
                    device.Type = updatedDevice.Type;
                    device.powerStatus = updatedDevice.powerStatus;
                    device.isConnected = updatedDevice.isConnected;

                    if (device is Camera camera && updatedDevice is Camera updatedCamera)
                    {
                        camera.isRecording = updatedCamera.isRecording;
                        camera.resolution = updatedCamera.resolution;
                        camera.angle = updatedCamera.angle;
                    }
                    else if (device is GarageDoor garageDoor && updatedDevice is GarageDoor updatedGarageDoor)
                    {
                        garageDoor.status = updatedGarageDoor.status;
                        garageDoor.isLocked = updatedGarageDoor.isLocked;
                        garageDoor.lastOpened = updatedGarageDoor.lastOpened;
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
    }
}
