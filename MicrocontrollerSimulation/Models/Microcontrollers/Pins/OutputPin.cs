using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontroller.Pins.PinConfiguration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontroller.Pins
{
    public class OutputPin : PinBase
    {
        public OutputPin(int number) : base(number)
        {
        }

        public override void UpdateSignal()
        {
            if (Function is null)
            {
                Signal = false;
                return;
            }

            var config = OutputFunctionInputPinsConfig!;
            var pins = config.Pins;

            if (pins is null)
            {
                Signal = false;
                return;
            }

            var inputs = Function.Expression.Inputs;

            foreach (var input in inputs)
            {
                 var entry = OutputFunctionInputPinsConfig!.
                    ConfigEntries.
                    Where(e => e.Input == input).
                    FirstOrDefault();

                var pin = pins.Where(p => p.Number == entry!.PinNumber).FirstOrDefault();

                input.Value = pin is not null && pin.Signal;
            }

            Signal = Function.Expression.Result;
        }
    }
}
