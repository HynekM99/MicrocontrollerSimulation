using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices
{
    public class ButtonDevice : InputDevice
    {
        public const string NAME = "Tlačítko";

        public override string Name { get { return NAME; } }

        public bool Pressed
        {
            get => Signal;
            set => Signal = value;
        }

        public override string ToString()
        {
            return NAME;
        }
    }
}
