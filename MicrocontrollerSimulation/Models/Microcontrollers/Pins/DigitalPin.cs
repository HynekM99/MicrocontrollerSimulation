using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontrollers.Pins
{
    public class DigitalPin
    {
        public event Action? PinModeChanged;
        public event Action? SignalChanged;
        public event Action? InputDeviceChanged;
        public event Action? FunctionConfigChanged;

        private readonly DigitalPin[] _pins;

        public int Number { get; }

        private PinMode _pinMode = PinMode.Input;
        public PinMode PinMode
        {
            get { return _pinMode; }
            set
            {
                if (_pinMode != value)
                {
                    _pinMode = value;
                    PinModeChanged?.Invoke();
                }
            }
        }

        private bool _signal = false;
        public bool Signal
        {
            get { return _signal; }
            private set
            {
                if (_signal != value)
                {
                    _signal = value;
                    SignalChanged?.Invoke();
                }
            }
        }

        private InputDevice? _inputDevice;
        public InputDevice? InputDevice
        {
            get => _inputDevice;
            set
            {
                if (_inputDevice is not null)
                {
                    _inputDevice.DeviceEdited -= OnDeviceEdited;
                }

                if (value is not null)
                {
                    value.DeviceEdited += OnDeviceEdited;
                }

                if (_inputDevice != value)
                {
                    _inputDevice = value;
                    InputDeviceChanged?.Invoke();
                }
            }
        }

        private FunctionConfig? _functionConfig;
        public FunctionConfig? FunctionConfig
        {
            get { return _functionConfig; }
            set
            {
                if (_functionConfig is not null)
                {
                    _functionConfig.ConfigChanged -= OnFunctionConfigChanged;
                }
                _functionConfig = value;

                if (_functionConfig is not null)
                {
                    _functionConfig.ConfigChanged += OnFunctionConfigChanged;
                }

                OnFunctionConfigChanged();
            }
        }

        public DigitalPin(int number, DigitalPin[] pins)
        {
            Number = number;
            _pins = pins;
        }

        public void UpdateSignal()
        {
            if (PinMode == PinMode.Input)
            {
                Signal = InputDevice is not null && InputDevice.Signal;
                return;
            }

            if (FunctionConfig is null)
            {
                Signal = false;
                return;
            }

            var config = FunctionConfig;
            var function = FunctionConfig.Function;

            if (function is null || _pins is null)
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

                var pin = _pins.Where(p => p.Number == entry!.PinNumber).FirstOrDefault();

                input.Value = pin is not null && pin.Signal;
            }

            Signal = function.Expression.Result;
        }

        private void OnDeviceEdited()
        {
            InputDeviceChanged?.Invoke();
        }

        private void OnFunctionConfigChanged()
        {
            if (FunctionConfig is not null &&
                FunctionConfig.Function is null)
            {
                FunctionConfig.Dispose();
                FunctionConfig = null;
                return;
            }

            FunctionConfigChanged?.Invoke();
        }
    }
}
