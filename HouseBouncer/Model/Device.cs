namespace HouseBouncer.Models
{
    public class DeviceModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }

        public int roomId { get; set; }

        public bool powerStatus { get; set; }

        public bool isConnected { get; set; }
        // Add additional properties as needed
    }
}
