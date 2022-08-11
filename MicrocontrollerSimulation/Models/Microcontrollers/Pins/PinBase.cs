using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontroller.Pins.PinConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontroller.Pins
{
    public abstract class PinBase
    {
        public event Action<bool>? SignalChanged;
        public event Action<InputDevice?>? InputDeviceChanged;
        public event Action<Function?>? FunctionChanged;

        public int Number { get; }

        private bool _signal = false;
        public bool Signal
        {
            get { return _signal; }
            protected set
            {
                _signal = value;
                SignalChanged?.Invoke(value);
            }
        }

        private InputDevice? _inputDevice;
        public InputDevice? InputDevice
        {
            get => _inputDevice;
            set
            {
                _inputDevice = value;
                InputDeviceChanged?.Invoke(value);
            }
        }

        private Function? _function;
        public Function? Function
        {
            get { return _function; }
            set
            {
                _function = value;
                OutputFunctionInputPinsConfig = value is null ? null : new(value);
                FunctionChanged?.Invoke(value);
            }
        }

        private OutputFunctionInputPinsConfig? _outputFunctionInputPinsConfig;
        public OutputFunctionInputPinsConfig? OutputFunctionInputPinsConfig
        {
            get { return _outputFunctionInputPinsConfig; }
            protected set { _outputFunctionInputPinsConfig = value; }
        }

        protected PinBase(int number)
        {
            Number = number;
        }

        public abstract void UpdateSignal();
    }
}
