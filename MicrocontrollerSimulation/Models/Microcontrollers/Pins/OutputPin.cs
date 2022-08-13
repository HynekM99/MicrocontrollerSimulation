using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontroller.Pins.Configuration;
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
            if (FunctionConfig is null)
            {
                Signal = false;
                return;
            }

            var config = FunctionConfig!;
            var function = FunctionConfig.Function;
            var pins = config.Pins;

            if (function is null || pins is null)
            {
                Signal = false;
                return;
            }

            var inputs = function.Expression.Inputs;

            foreach (var input in inputs)
            {
                 var entry = FunctionConfig!.
                    ConfigEntries!.
                    Where(e => e.Input == input).
                    FirstOrDefault();

                var pin = pins.Where(p => p.Number == entry!.PinNumber).FirstOrDefault();

                input.Value = pin is not null && pin.Signal;
            }

            Signal = function.Expression.Result;
        }
    }
}
