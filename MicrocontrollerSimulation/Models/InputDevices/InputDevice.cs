using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.InputDevices
{
    public abstract class InputDevice
    {
        public event Action<bool>? SignalChanged;

        public abstract string Name { get; }

        private bool _signal = false;
        public bool Signal
        {
            get => _signal;
            protected set
            {
                if (_signal != value)
                {
                    _signal = value;
                    SignalChanged?.Invoke(value);
                }
            }
        }
    }
}
