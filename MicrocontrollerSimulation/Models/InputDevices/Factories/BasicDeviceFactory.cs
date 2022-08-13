using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices.Factories
{
    public class BasicDeviceFactory : IDeviceFactory
    {
        public ButtonDevice CreateButtonDevice()
        {
            return new ButtonDevice();
        }

        public SwitchDevice CreateSwitchDevice()
        {
            return new SwitchDevice();
        }

        public ClockDevice CreateClockDevice()
        {
            return new ClockDevice();
        }

        public List<string> GetAvailableDevices()
        {
            return new List<string>
            {
                ButtonDevice.NAME,
                SwitchDevice.NAME,
                ClockDevice.NAME
            };
        }
    }
}
