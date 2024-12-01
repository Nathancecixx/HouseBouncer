using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SmartLockTesting.Dependencies;

namespace SmartLockTesting
{
    [TestClass]
    public class DeviceModelTests
    {
        [TestMethod]
        public void DeviceModel_Attributes_InitializeCorrectly()
        {
            // Arrange
            string deviceName = "Smart Light";
            int deviceId = 101;
            string deviceType = "Light";
            string roomId = "1";
            bool powerStatus = true;
            bool isConnected = true;

            // Act
            var device = new DeviceModel(deviceName, deviceId, deviceType, roomId, powerStatus, isConnected);

            // Assert
            Assert.AreEqual(deviceName, device.Name, "Device name not initialized correctly.");
            Assert.AreEqual(deviceId, device.Id, "Device ID not initialized correctly.");
            Assert.AreEqual(deviceType, device.Type, "Device type not initialized correctly.");
            Assert.AreEqual(roomId, device.RoomId, "Room ID not initialized correctly.");
            Assert.AreEqual(powerStatus, device.PowerStatus, "Power status not initialized correctly.");
            Assert.AreEqual(isConnected, device.IsConnected, "Connection status not initialized correctly.");
        }

        [TestMethod]
        public void DeviceModel_GettersSetters_WorkCorrectly()
        {
            // Arrange
            var device = new DeviceModel("Device", 0, "Type", "Room");
            string newName = "Smart Light";
            int newId = 101;
            string newType = "Light";
            string newRoomId = "1";
            bool newPowerStatus = false;
            bool newIsConnected = false;

            // Act
            device.Name = newName;
            device.Id = newId;
            device.Type = newType;
            device.RoomId = newRoomId;
            device.PowerStatus = newPowerStatus;
            device.IsConnected = newIsConnected;

            // Assert
            Assert.AreEqual(newName, device.Name, "Name setter or getter not working correctly.");
            Assert.AreEqual(newId, device.Id, "Id setter or getter not working correctly.");
            Assert.AreEqual(newType, device.Type, "Type setter or getter not working correctly.");
            Assert.AreEqual(newRoomId, device.RoomId, "RoomId setter or getter not working correctly.");
            Assert.AreEqual(newPowerStatus, device.PowerStatus, "PowerStatus setter or getter not working correctly.");
            Assert.AreEqual(newIsConnected, device.IsConnected, "IsConnected setter or getter not working correctly.");
        }
    }

    [TestClass]
    public class CameraTests
    {
        [TestMethod]
        public void Camera_Attributes_InitializeCorrectly()
        {
            // Arrange
            string deviceName = "Front Door Camera";
            int deviceId = 601;
            string roomId = "6";
            bool powerStatus = true;
            bool isRecording = false;
            string resolution = "1080p";
            float angle = 0.0f;
            string storagePath = "/recordings/front_door";

            // Act
            var camera = new Camera(deviceName, deviceId, roomId, powerStatus)
            {
                IsRecording = isRecording,
                Resolution = resolution,
                Angle = angle,
                StoragePath = storagePath
            };

            // Assert
            Assert.AreEqual(deviceName, camera.Name, "Camera name not initialized correctly.");
            Assert.AreEqual(deviceId, camera.Id, "Camera ID not initialized correctly.");
            Assert.AreEqual("Camera", camera.Type, "Camera type not initialized correctly.");
            Assert.AreEqual(roomId, camera.RoomId, "Room ID not initialized correctly.");
            Assert.AreEqual(powerStatus, camera.PowerStatus, "Power status not initialized correctly.");
            Assert.AreEqual(isRecording, camera.IsRecording, "IsRecording not initialized correctly.");
            Assert.AreEqual(resolution, camera.Resolution, "Resolution not initialized correctly.");
            Assert.AreEqual(angle, camera.Angle, "Angle not initialized correctly.");
            Assert.AreEqual(storagePath, camera.StoragePath, "StoragePath not initialized correctly.");
        }

        [TestMethod]
        public void Camera_StartStopRecording_WorksCorrectly()
        {
            // Arrange
            var camera = new Camera("Test Camera", 100, "1", true)
            {
                IsRecording = false,
                StoragePath = "/recordings/test1"
            };

            // Act
            camera.StartRecording();

            // Assert
            Assert.IsTrue(camera.IsRecording, "Camera did not start recording.");

            // Act
            camera.StopRecording();

            // Assert
            Assert.IsFalse(camera.IsRecording, "Camera did not stop recording.");
        }

        [TestMethod]
        public void Camera_AdjustAngle_UpdatesAngle()
        {
            // Arrange
            var camera = new Camera("Test Camera", 100, "1", true);
            float newAngle = 45.0f;

            // Act
            camera.AdjustAngle(newAngle);

            // Assert
            Assert.AreEqual(newAngle, camera.Angle, "Camera angle did not update correctly.");
        }
    }

    [TestClass]
    public class GarageDoorTests
    {
        [TestMethod]
        public void GarageDoor_Attributes_InitializeCorrectly()
        {
            // Arrange
            string deviceName = "Garage Door";
            int deviceId = 601;
            string roomId = "6";
            bool powerStatus = true;
            string status = "Locked";
            bool isLocked = false;
            DateTime lastOpened = DateTime.Now;

            // Act
            var garageDoor = new GarageDoor(deviceName, deviceId, roomId, powerStatus)
            {
                Status = status,
                IsLocked = isLocked,
                LastOpened = lastOpened
            };

            // Assert
            Assert.AreEqual(deviceName, garageDoor.Name, "GarageDoor name not initialized correctly.");
            Assert.AreEqual(deviceId, garageDoor.Id, "GarageDoor ID not initialized correctly.");
            Assert.AreEqual("Garage Door", garageDoor.Type, "GarageDoor type not initialized correctly.");
            Assert.AreEqual(roomId, garageDoor.RoomId, "Room ID not initialized correctly.");
            Assert.AreEqual(powerStatus, garageDoor.PowerStatus, "Power status not initialized correctly.");
            Assert.AreEqual(status, garageDoor.Status, "Status not initialized correctly.");
            Assert.AreEqual(isLocked, garageDoor.IsLocked, "IsLocked not initialized correctly.");
            Assert.AreEqual(lastOpened, garageDoor.LastOpened, "LastOpened not initialized correctly.");
        }

        [TestMethod]
        public void GarageDoor_OpenCloseDoor_WorksCorrectly()
        {
            // Arrange
            var garageDoor = new GarageDoor("Garage Door", 601, "6", true)
            {
                IsLocked = false,
                Status = "Closed"
            };

            // Act
            garageDoor.OpenDoor();
            
            // Assert
            Assert.AreEqual("Open", garageDoor.Status, "GarageDoor did not open correctly.");
            Assert.AreEqual(DateTime.Now.Date, garageDoor.LastOpened.Date, "GarageDoor LastOpened not updated correctly.");

            // Act
            garageDoor.CloseDoor();

            // Assert
            Assert.AreEqual("Closed", garageDoor.Status, "GarageDoor did not close correctly.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GarageDoor_OpenDoor_WhenLocked_ThrowsException()
        {
            // Arrange
            var garageDoor = new GarageDoor("Garage Door", 601, "6", true)
            {
                IsLocked = true,
                Status = "Closed"
            };

            // Act
            garageDoor.OpenDoor();

            // Assert handled by ExpectedException
        }
    }
}
