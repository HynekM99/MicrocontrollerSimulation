using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices.Factories
{
    public interface IDeviceFactory
    {
        ButtonDevice CreateButtonDevice();
        SwitchDevice CreateSwitchDevice();
        ClockDevice CreateClockDevice();

        List<string> GetAvailableDevices();
        InputDevice? CreateDevice(string? deviceName)
        {
            return deviceName switch
            {
                ButtonDevice.NAME => CreateButtonDevice(),
                SwitchDevice.NAME => CreateSwitchDevice(),
                ClockDevice.NAME => CreateClockDevice(),
                _ => null
            };
        }
    }
}
