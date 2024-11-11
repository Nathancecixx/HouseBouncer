using System.Collections.ObjectModel;

namespace HouseBouncer.Models
{
    public class Room
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<DeviceModel> Devices { get; set; }

        public Room()
        {
            Devices = new ObservableCollection<DeviceModel>();
        }
    }
}
