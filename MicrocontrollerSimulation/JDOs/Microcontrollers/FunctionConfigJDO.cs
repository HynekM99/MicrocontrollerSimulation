using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs.Microcontrollers
{
    public class FunctionConfigJDO
    {
        public string Function { get; set; } = "";
        public List<ConfigEntryJDO> ConfigEntries { get; set; } = new();
    }
}
