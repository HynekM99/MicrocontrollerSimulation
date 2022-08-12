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

        public InputDevice CreateDevice(string? deviceType)
        {
            return deviceType switch
            {
                nameof(ButtonDevice) => CreateButtonDevice(),
                nameof(SwitchDevice) => CreateSwitchDevice(),
                nameof(ClockDevice) => CreateClockDevice(),
                _ => throw new ArgumentException($"Device of type \"{deviceType}\" does not exist.")
            };
        }
    }
}
