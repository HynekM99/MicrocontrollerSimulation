using MicrocontrollerSimulation.JDOs.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs.Microcontrollers
{
    public class DigitalPinJDO
    {
        public int Number { get; set; } = -1;
        public bool OutputMode { get; set; } = false;
        public InputDeviceJDO? InputDevice { get; set; } = null;
        public FunctionConfigJDO? FunctionConfig { get; set; } = null;
    }
}
