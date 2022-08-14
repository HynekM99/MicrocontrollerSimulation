using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration
{
    public class ConfigEntry
    {
        public event Action<ConfigEntry>? PinNumberChanged;

        public Input Input { get; }

        private int? _pinNumber;
        public int? PinNumber
        {
            get { return _pinNumber; }
            set
            {
                _pinNumber = value;
                if (value < 0) _pinNumber = null;
                PinNumberChanged?.Invoke(this);
            }
        }

        public ConfigEntry(Input input, int? pin)
        {
            Input = input;
            PinNumber = pin;
        }

        public ConfigEntry(Input input) : this(input, null)
        {

        }

        public override bool Equals(object? obj)
        {
            return obj is ConfigEntry entry &&
                   Input.AsString == Input.AsString &&
                   PinNumber == entry.PinNumber;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Input, PinNumber);
        }
    }
}
