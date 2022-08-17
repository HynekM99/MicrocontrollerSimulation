using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs.InputDevices
{
    public class ClockDeviceJDO : InputDeviceJDO
    {
        public int Frequency { get; set; } = 1;
    }
}
