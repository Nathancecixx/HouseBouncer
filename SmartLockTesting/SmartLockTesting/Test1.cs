using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using SmartLockTesting.Dependencies;

namespace SmartLockTesting
{
    [TestClass]
    public class FanTests
    {
        [TestMethod]
        public void Fan_Speed_SetterGetter_WorksCorrectly()
        {
            // Arrange
            var fan = new Fan();

            // Act
            fan.Speed = 3;

            // Assert
            Assert.AreEqual(3, fan.Speed, "Fan speed getter or setter did not work correctly.");
        }

        [TestMethod]
        public void Fan_Mode_DefaultsToNormal()
        {
            // Arrange
            var fan = new Fan();

            // Assert
            Assert.AreEqual("Normal", fan.Mode, "Fan mode did not default to 'Normal'.");
        }
    }

    [TestClass]
    public class FridgeTests
    {
        [TestMethod]
        public void Fridge_Temperature_SetterGetter_WorksCorrectly()
        {
            // Arrange
            var fridge = new Fridge();

            // Act
            fridge.Temperature = 5.5f;

            // Assert
            Assert.AreEqual(5.5f, fridge.Temperature, "Fridge temperature getter or setter did not work correctly.");
        }

        [TestMethod]
        public void Fridge_DoorStatus_DefaultsToFalse()
        {
            // Arrange
            var fridge = new Fridge();

            // Assert
            Assert.IsFalse(fridge.IsDoorOpen, "Fridge door status did not default to false.");
        }
    }

    [TestClass]
    public class SmartLockTests
    {
        [TestMethod]
        public void SmartLock_Lock_UpdatesIsLockedAndLastAccessed()
        {
            // Arrange
            var smartLock = new SmartLock();

            // Act
            smartLock.Lock();

            // Assert
            Assert.IsTrue(smartLock.IsLocked, "SmartLock did not lock correctly.");
            Assert.AreEqual(DateTime.Now.Date, smartLock.LastAccessed.Date, "SmartLock LastAccessed not updated correctly.");
        }

        [TestMethod]
        public void SmartLock_ToggleUnlock_WithCorrectPin_Unlocks()
        {
            // Arrange
            var smartLock = new SmartLock();

            // Act
            var result = smartLock.ToggleUnlock("3765");

            // Assert
            Assert.IsTrue(result, "SmartLock did not unlock with correct PIN.");
            Assert.IsFalse(smartLock.IsLocked, "SmartLock IsLocked state did not toggle.");
        }

        [TestMethod]
        public void SmartLock_ToggleUnlock_WithIncorrectPin_DoesNotUnlock()
        {
            // Arrange
            var smartLock = new SmartLock();

            // Act
            var result = smartLock.ToggleUnlock("1234");

            // Assert
            Assert.IsFalse(result, "SmartLock unlocked with incorrect PIN.");
            Assert.IsTrue(smartLock.IsLocked, "SmartLock IsLocked state toggled with incorrect PIN.");
        }
    }
}
