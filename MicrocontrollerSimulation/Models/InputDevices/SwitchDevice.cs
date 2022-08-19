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

        public bool IsToggled
        {
            get { return Signal; }
            set { Signal = value; }
        }

        public override string ToString()
        {
            return NAME;
        }
    }
}
