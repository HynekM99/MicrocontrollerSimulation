using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices
{
    public class SwitchDevice : InputDevice
    {
        public const string NAME = "Přepínač";

        public override string Name { get { return NAME; } }

        public void Toggle() => Signal = !Signal;

        public override string ToString()
        {
            return NAME;
        }
    }
}
