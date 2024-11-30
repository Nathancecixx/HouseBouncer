using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace HouseBouncer.Models
{
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "Json-Type")]
    [JsonDerivedType(typeof(GarageDoor), "Garage Door")]
    [JsonDerivedType(typeof(Camera), "Camera")]
    [JsonDerivedType(typeof(Fan), "Fan")]
    [JsonDerivedType(typeof(Fridge), "Fridge")]
    public class DeviceModel : INotifyPropertyChanged
    {
        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        private string name;
        public string Name
        {
            get => name;
            set
            {
                if (name != value)
                {
                    name = value;
                    OnPropertyChanged();
                }
            }
        }

        private int id;
        public int Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged();
                }
            }
        }

        private string type;
        public string Type
        {
            get => type;
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        private string roomId;
        public string RoomId
        {
            get => roomId;
            set
            {
                if (roomId != value)
                {
                    roomId = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool powerStatus;
        public bool PowerStatus
        {
            get => powerStatus;
            set
            {
                if (powerStatus != value)
                {
                    powerStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isConnected;
        public bool IsConnected
        {
            get => isConnected;
            set
            {
                if (isConnected != value)
                {
                    isConnected = value;
                    OnPropertyChanged();
                }
            }
        }
    }

    public class Camera : DeviceModel
    {
        private bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set
            {
                if (isRecording != value)
                {
                    isRecording = value;
                    OnPropertyChanged();
                }
            }
        }

        private string resolution;
        public string Resolution
        {
            get => resolution;
            set
            {
                if (resolution != value)
                {
                    resolution = value;
                    OnPropertyChanged();
                }
            }
        }

        private float angle;
        public float Angle
        {
            get => angle;
            set
            {
                if (angle != value)
                {
                    angle = value;
                    OnPropertyChanged();
                }
            }
        }

        private string storagePath;
        public string StoragePath
        {
            get => storagePath;
            set
            {
                if (storagePath != value)
                {
                    storagePath = value;
                    OnPropertyChanged();
                }
            }
        }

        public Camera()
        {
            Type = "Camera";
            IsRecording = false;
            Resolution = "1080p";
            Angle = 0;
            StoragePath = "../Recordings";
        }
    }

    public class GarageDoor : DeviceModel
    {
        private string status;
        public string Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isLocked;
        public bool IsLocked
        {
            get => isLocked;
            set
            {
                if (isLocked != value)
                {
                    isLocked = value;
                    OnPropertyChanged();
                }
            }
        }

        private DateTime lastOpened;
        public DateTime LastOpened
        {
            get => lastOpened;
            set
            {
                if (lastOpened != value)
                {
                    lastOpened = value;
                    OnPropertyChanged();
                }
            }
        }

        public GarageDoor()
        {
            Type = "Garage Door";
            Status = "Unknown";
            IsLocked = false;
            LastOpened = DateTime.MinValue;
        }
    }

    public class Fan : DeviceModel
    {
        private int speed;
        public int Speed
        {
            get => speed;
            set
            {
                if (speed != value)
                {
                    speed = value;
                    OnPropertyChanged();
                }
            }
        }

        private string mode;
        public string Mode
        {
            get => mode;
            set
            {
                if (mode != value)
                {
                    mode = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isOscillating;
        public bool IsOscillating
        {
            get => isOscillating;
            set
            {
                if (isOscillating != value)
                {
                    isOscillating = value;
                    OnPropertyChanged();
                }
            }
        }

        public Fan()
        {
            Type = "Fan";
            Speed = 0;
            Mode = "Normal";
            IsOscillating = false;
        }
    }

    public class Fridge : DeviceModel
    {
        private float temperature;
        public float Temperature
        {
            get => temperature;
            set
            {
                if (temperature != value)
                {
                    temperature = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isDoorOpen;
        public bool IsDoorOpen
        {
            get => isDoorOpen;
            set
            {
                if (isDoorOpen != value)
                {
                    isDoorOpen = value;
                    OnPropertyChanged();
                }
            }
        }

        private string coolingMode;
        public string CoolingMode
        {
            get => coolingMode;
            set
            {
                if (coolingMode != value)
                {
                    coolingMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public Fridge()
        {
            Type = "Fridge";
            Temperature = 4.0f;
            IsDoorOpen = false;
            CoolingMode = "Eco";
        }
    }

    public class SmartLock : DeviceModel
    {
        public bool IsLocked { get; set; }
        public string PinCode { get; set; }
        public bool ToggleLock { get; set; }
        public DateTime LastAccessed { get; set; }

        public SmartLock()
        {
            Type = "Smart Lock";
            IsLocked = true;
            PinCode = "3765";
            ToggleLock = true;
            LastAccessed = DateTime.MinValue;
        }

        // Method to lock the smart lock
        public void Lock()
        {
            IsLocked = true;
            LastAccessed = DateTime.Now;
        }

        // Method to unlock the smart lock using a pin code
        public bool ToggleUnlock(string inputPin)
        {
            if (PinCode == inputPin)
            {
                IsLocked = !IsLocked;
                LastAccessed = DateTime.Now;
                return true;
            }
            else
            {
                return false; // Incorrect pin
            }
        }
    }


}
