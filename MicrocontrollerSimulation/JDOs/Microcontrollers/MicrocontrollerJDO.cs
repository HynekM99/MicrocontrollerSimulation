using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs.Microcontrollers
{
    public class MicrocontrollerJDO
    {
        public List<DigitalPinJDO> Pins { get; set; } = new();
    }
}
