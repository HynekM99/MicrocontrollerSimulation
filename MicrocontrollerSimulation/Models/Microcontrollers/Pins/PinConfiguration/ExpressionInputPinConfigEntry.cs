using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontroller.Pins.PinConfiguration
{
    public class ExpressionInputPinConfigEntry
    {
        public event Action<ExpressionInputPinConfigEntry>? PinNumberChanged;

        public Input Input { get; }

        private int? _pinNumber;
        public int? PinNumber
        {
            get { return _pinNumber; }
            set
            {
                _pinNumber = value;
                PinNumberChanged?.Invoke(this);
            }
        }

        public ExpressionInputPinConfigEntry(Input input, int? pin)
        {
            Input = input;
            PinNumber = pin;
        }

        public ExpressionInputPinConfigEntry(Input input) : this(input, null)
        {

        }
    }
}
