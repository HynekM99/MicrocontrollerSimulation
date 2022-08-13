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
    }
}
