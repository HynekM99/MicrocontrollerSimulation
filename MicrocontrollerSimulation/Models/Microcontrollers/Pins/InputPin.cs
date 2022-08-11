using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontroller.Pins
{
    public class InputPin : PinBase
    {
        public InputPin(int number) : base(number)
        {
        }

        public override void UpdateSignal()
        {
            Signal = InputDevice is not null && InputDevice.Signal;
        }
    }
}
