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

        public DataService()
        {
            Rooms = new ObservableCollection<Room>();
            // Initialize data from storage asynchronously
            LoadDataAsync();
        }

        public async Task LoadDataAsync()
        {
            var filePath = GetFilePath();
            if (File.Exists(filePath))
            {
                string json = await File.ReadAllTextAsync(filePath);
                var loadedRooms = JsonSerializer.Deserialize<ObservableCollection<Room>>(json);
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

                string json = JsonSerializer.Serialize(Rooms);
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
                        new DeviceModel { Id = 1, Name = "Thermostat", Type = "Climate", roomId = 1, powerStatus = true, isConnected = true },
                        new DeviceModel { Id = 2, Name = "Smart Light", Type = "Lighting", roomId = 1, powerStatus = false, isConnected = true }
                    }
                },
                new Room
                {
                    Id = "2",
                    Name = "Bedroom",
                    Devices = new ObservableCollection<DeviceModel>
                    {
                        new DeviceModel { Id = 3, Name = "Smart Speaker", Type = "Entertainment", roomId = 2, powerStatus = true, isConnected = false },
                        new DeviceModel { Id = 4, Name = "Air Purifier", Type = "Air Quality", roomId = 2, powerStatus = false, isConnected = true }
                    }
                }
            };
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
