using System.Diagnostics;
using System.Text.Json.Serialization;

namespace HouseBouncer.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Json-Type")]
    [JsonDerivedType(typeof(GarageDoor), "Garage Door")]
    [JsonDerivedType(typeof(Camera), "Camera")]
    public class DeviceModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
        public string roomId { get; set; }
        public bool powerStatus { get; set; }
        public bool isConnected { get; set; }
    }


    public class Camera : DeviceModel
    {
        public bool isRecording { get; set; }
        public string resolution { get; set; }
        public float angle { get; set; }
        public string storagePath { get; set; }
        public Camera()
        {
            Type = "Camera";
            isRecording = false;
            resolution = "1080p";
            angle = 0;
            storagePath = "../Recordings";
        }
    }


    public class GarageDoor : DeviceModel 
    {
        public string status { get; set; }
        public bool isLocked { get; set; }
        public DateTime lastOpened { get; set; }

        public GarageDoor()
        {
            Type = "Garage Door";
            status = "Unknown";
            isLocked = false;
            lastOpened = DateTime.MinValue;
        }

    
    }
}
