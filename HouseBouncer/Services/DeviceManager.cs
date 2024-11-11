using SmartHomeApp.Models;
using System.Collections.ObjectModel;

namespace SmartHomeApp.Services
{
    public class DeviceManager
    {
        public ObservableCollection<Room> GetRooms()
        {
            // Placeholder data. Replace with actual data retrieval logic.
            return new ObservableCollection<Room>
            {
                new Room
                {
                    Id = "1",
                    Name = "Living Room",
                    Devices = new ObservableCollection<DeviceModel>
                    {
                        new DeviceModel { Id = "1", Name = "Thermostat", Type = "Climate" },
                        new DeviceModel { Id = "2", Name = "Smart Light", Type = "Lighting" }
                    }
                },
                new Room
                {
                    Id = "2",
                    Name = "Bedroom",
                    Devices = new ObservableCollection<DeviceModel>
                    {
                        new DeviceModel { Id = "3", Name = "Smart Speaker", Type = "Entertainment" },
                        new DeviceModel { Id = "4", Name = "Air Purifier", Type = "Air Quality" }
                    }
                }
            };
        }
    }
}
