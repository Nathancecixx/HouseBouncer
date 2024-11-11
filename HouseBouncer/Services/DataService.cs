using HouseBouncer.Models;
using System.Collections.ObjectModel;

namespace HouseBouncer.Services
{
    public class DataService
    {
        private ObservableCollection<Room> rooms;

        public DataService()
        {
            // Initialize with some placeholder data
            rooms = new ObservableCollection<Room>
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

        public ObservableCollection<Room> GetRooms()
        {
            return rooms;
        }

        public void AddRoom(Room newRoom)
        {
            rooms.Add(newRoom);
        }

        // Optionally, implement methods to save and load data from persistent storage
    }
}
