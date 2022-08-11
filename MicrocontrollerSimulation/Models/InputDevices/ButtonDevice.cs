using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices
{
    public class ButtonDevice : InputDevice
    {
        public bool Pressed
        {
            get => Signal;
            set => Signal = value;
        }

        public override string ToString()
        {
            return "Tlačítko";
        }
    }
}
