// DataService.cs
using HouseBouncer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows; // If using WPF for Dispatcher

namespace HouseBouncer.Services
{
    public class DataService : IDisposable
    {
        private const string DataFolderName = "HouseBouncer";
        private const string RoomsFolderName = "Rooms";
        private ObservableCollection<Room> rooms;

        public ObservableCollection<Room> Rooms
        {
            get => rooms;
            private set => rooms = value;
        }

        private readonly JsonSerializerOptions _jsonOptions;

        // Dictionary to hold FileSystemWatchers for each device file
        private readonly Dictionary<string, FileSystemWatcher> _deviceWatchers = new Dictionary<string, FileSystemWatcher>();

        public DataService()
        {
            Rooms = new ObservableCollection<Room>();
            _jsonOptions = new JsonSerializerOptions
            {
                // Ensure polymorphic serialization is respected
                WriteIndented = true,
                // Add other options if necessary
                Converters = { new JsonStringEnumConverter() }
            };
            Rooms.CollectionChanged += OnRoomsCollectionChanged;
            // Initialize data from storage asynchronously
            LoadDataAsync().ConfigureAwait(false);
        }

        // Loads device data from all room folders
        public async Task LoadDataAsync()
        {
            string basePath = GetRoomsDirectoryPath();
            if (Directory.Exists(basePath))
            {
                var roomDirectories = Directory.GetDirectories(basePath);
                foreach (var roomDir in roomDirectories)
                {
                    string roomId = Path.GetFileName(roomDir);
                    string roomNamePath = Path.Combine(roomDir, "RoomName.txt");
                    string roomName = roomId; 

                    if (File.Exists(roomNamePath))
                    {
                        roomName = await File.ReadAllTextAsync(roomNamePath);
                    }

                    var room = new Room
                    {
                        Id = roomId,
                        Name = roomName,
                        Devices = new ObservableCollection<DeviceModel>()
                    };

                    room.Devices.CollectionChanged += OnDevicesCollectionChanged;

                    var deviceFiles = Directory.GetFiles(roomDir, "Device_*.json");
                    foreach (var deviceFile in deviceFiles)
                    {
                        try
                        {
                            string json = await File.ReadAllTextAsync(deviceFile);
                            var device = JsonSerializer.Deserialize<DeviceModel>(json, _jsonOptions);
                            if (device != null)
                            {
                                room.Devices.Add(device);
                                SubscribeToDeviceChanges(device);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error loading device from {deviceFile}: {ex.Message}");
                        }
                    }

                    Rooms.Add(room);
                }
            }
        }

        // Saves an individual device to its file
        public async Task SaveDeviceAsync(string roomId, DeviceModel device)
        {
            try
            {
                string deviceFilePath = GetDeviceFilePath(roomId, device.Id);
                string json = JsonSerializer.Serialize(device, _jsonOptions);
                await File.WriteAllTextAsync(deviceFilePath, json);
                Console.WriteLine($"Saved device {device.Id} to {deviceFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving device {device.Id}: {ex.Message}");
            }
        }

        // Updates the in-memory device model when properties change and saves to file.
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
                        camera.StoragePath = updatedCamera.StoragePath;
                    }
                    else if (device is GarageDoor garageDoor && updatedDevice is GarageDoor updatedGarageDoor)
                    {
                        garageDoor.Status = updatedGarageDoor.Status;
                        garageDoor.IsLocked = updatedGarageDoor.IsLocked;
                        garageDoor.LastOpened = updatedGarageDoor.LastOpened;
                    }
                    else if (device is Fan fan && updatedDevice is Fan updatedFan)
                    {
                        fan.Speed = updatedFan.Speed;
                        fan.Mode = updatedFan.Mode;
                        fan.IsOscillating = updatedFan.IsOscillating;
                    }
                    else if (device is Fridge fridge && updatedDevice is Fridge updatedFridge)
                    {
                        fridge.Temperature = updatedFridge.Temperature;
                        fridge.IsDoorOpen = updatedFridge.IsDoorOpen;
                        fridge.CoolingMode = updatedFridge.CoolingMode;
                    }

                    // Save the updated device
                    _ = SaveDeviceAsync(room.Id, device);
                }
            }
        }

        // Creates a new room folder
        public async Task AddRoomAsync(Room newRoom)
        {
            Rooms.Add(newRoom);
            string roomDir = GetRoomDirectoryPath(newRoom.Id);
            Directory.CreateDirectory(roomDir);

            // Save room name to a text file
            string roomNamePath = Path.Combine(roomDir, "RoomName.txt");
            await File.WriteAllTextAsync(roomNamePath, newRoom.Name);

            newRoom.Devices.CollectionChanged += OnDevicesCollectionChanged;
        }


        // Deletes a room folder
        public void RemoveRoom(Room room)
        {
            if (Rooms.Remove(room))
            {
                string roomDir = GetRoomDirectoryPath(room.Id);
                if (Directory.Exists(roomDir))
                {
                    Directory.Delete(roomDir, true);
                }

                // Dispose and remove any watchers related to devices in this room
                foreach (var device in room.Devices)
                {
                    string deviceFilePath = GetDeviceFilePath(room.Id, device.Id);
                    if (_deviceWatchers.ContainsKey(deviceFilePath))
                    {
                        _deviceWatchers[deviceFilePath].Dispose();
                        _deviceWatchers.Remove(deviceFilePath);
                    }
                }
            }
        }


        // Creates a new device file in a room folder
        public async Task AddDeviceAsync(string roomId, DeviceModel newDevice)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null)
            {
                room.Devices.Add(newDevice);
                SubscribeToDeviceChanges(newDevice);
                await SaveDeviceAsync(roomId, newDevice);
            }
        }


        // Deletes a device file from a room folder
        public void RemoveDevice(string roomId, DeviceModel device)
        {
            var room = Rooms.FirstOrDefault(r => r.Id == roomId);
            if (room != null && room.Devices.Remove(device))
            {
                string deviceFilePath = GetDeviceFilePath(roomId, device.Id);
                if (File.Exists(deviceFilePath))
                {
                    File.Delete(deviceFilePath);
                }

                if (_deviceWatchers.ContainsKey(deviceFilePath))
                {
                    _deviceWatchers[deviceFilePath].Dispose();
                    _deviceWatchers.Remove(deviceFilePath);
                }
            }
        }


        // Handles changes in list of rooms
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
                    foreach (var device in room.Devices)
                    {
                        device.PropertyChanged -= OnDevicePropertyChanged;
                        string deviceFilePath = GetDeviceFilePath(room.Id, device.Id);
                        if (_deviceWatchers.ContainsKey(deviceFilePath))
                        {
                            _deviceWatchers[deviceFilePath].Dispose();
                            _deviceWatchers.Remove(deviceFilePath);
                        }
                    }
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
                    string deviceFilePath = GetDeviceFilePath(device.RoomId, device.Id);
                    if (_deviceWatchers.ContainsKey(deviceFilePath))
                    {
                        _deviceWatchers[deviceFilePath].Dispose();
                        _deviceWatchers.Remove(deviceFilePath);
                    }
                }
            }
        }

        private void SubscribeToDeviceChanges(DeviceModel device)
        {
            device.PropertyChanged += OnDevicePropertyChanged;

            string deviceFilePath = GetDeviceFilePath(device.RoomId, device.Id);
            string deviceDirectory = Path.GetDirectoryName(deviceFilePath);
            string deviceFileName = Path.GetFileName(deviceFilePath);

            if (!_deviceWatchers.ContainsKey(deviceFilePath))
            {
                var watcher = new FileSystemWatcher
                {
                    Path = deviceDirectory,
                    Filter = deviceFileName,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.Attributes
                };

                watcher.Changed += async (s, e) => await OnDeviceFileChanged(s, e, device);
                watcher.EnableRaisingEvents = true;

                _deviceWatchers.Add(deviceFilePath, watcher);
            }
        }


        private async Task OnDeviceFileChanged(object sender, FileSystemEventArgs e, DeviceModel device)
        {
            // Prevent multiple events from firing
            FileSystemWatcher watcher = sender as FileSystemWatcher;
            if (watcher == null) return;

            // Delay to ensure the file write operation is complete
            await Task.Delay(100);

            try
            {
                string json = await ReadFileAsyncWithRetry(e.FullPath, 3, TimeSpan.FromMilliseconds(100));
                var updatedDevice = JsonSerializer.Deserialize<DeviceModel>(json, _jsonOptions);
                if (updatedDevice != null)
                {
                    // Update device properties on the UI thread if necessary
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        device.Name = updatedDevice.Name;
                        device.Type = updatedDevice.Type;
                        device.PowerStatus = updatedDevice.PowerStatus;
                        device.IsConnected = updatedDevice.IsConnected;

                        switch (device)
                        {
                            case Camera camera when updatedDevice is Camera updatedCamera:
                                camera.IsRecording = updatedCamera.IsRecording;
                                camera.Resolution = updatedCamera.Resolution;
                                camera.Angle = updatedCamera.Angle;
                                camera.StoragePath = updatedCamera.StoragePath;
                                break;
                            case GarageDoor garageDoor when updatedDevice is GarageDoor updatedGarageDoor:
                                garageDoor.Status = updatedGarageDoor.Status;
                                garageDoor.IsLocked = updatedGarageDoor.IsLocked;
                                garageDoor.LastOpened = updatedGarageDoor.LastOpened;
                                break;
                            case Fan fan when updatedDevice is Fan updatedFan:
                                fan.Speed = updatedFan.Speed;
                                fan.Mode = updatedFan.Mode;
                                fan.IsOscillating = updatedFan.IsOscillating;
                                break;
                            case Fridge fridge when updatedDevice is Fridge updatedFridge:
                                fridge.Temperature = updatedFridge.Temperature;
                                fridge.IsDoorOpen = updatedFridge.IsDoorOpen;
                                fridge.CoolingMode = updatedFridge.CoolingMode;
                                break;
                                // Handle other device types similarly
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating device from file: {ex.Message}");
            }
        }


        private async Task<string> ReadFileAsyncWithRetry(string filePath, int maxRetries, TimeSpan delay)
        {
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (var reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
                catch (IOException)
                {
                    await Task.Delay(delay);
                }
            }

            throw new IOException($"Unable to read file {filePath} after {maxRetries} attempts.");
        }


        private async void OnDevicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is DeviceModel updatedDevice)
            {
                UpdateDevice(updatedDevice);
            }
        }


        private string GetRoomsDirectoryPath()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string basePath = Path.Combine(localAppData, DataFolderName, RoomsFolderName);
            return basePath;
        }


        private string GetRoomDirectoryPath(string roomId)
        {
            return Path.Combine(GetRoomsDirectoryPath(), roomId);
        }


        private string GetDeviceFilePath(string roomId, int deviceId)
        {
            string roomDir = GetRoomDirectoryPath(roomId);
            return Path.Combine(roomDir, $"Device_{deviceId}.json");
        }


        // Disposes all FileSystemWatchers to prevent memory leaks.
        public void Dispose()
        {
            foreach (var watcher in _deviceWatchers.Values)
            {
                watcher.Dispose();
            }
            _deviceWatchers.Clear();
        }
    }
}
