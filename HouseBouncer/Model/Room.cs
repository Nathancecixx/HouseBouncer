// Room.cs
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace HouseBouncer.Models
{
    public class Room : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string id;
        public string Id
        {
            get => id;
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
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
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        private ObservableCollection<DeviceModel> devices = new ObservableCollection<DeviceModel>();
        public ObservableCollection<DeviceModel> Devices
        {
            get => devices;
            set
            {
                if (devices != value)
                {
                    devices = value;
                    OnPropertyChanged(nameof(Devices));
                }
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
