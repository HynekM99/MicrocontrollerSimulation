using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Provider;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontrollers.Pins
{
    public abstract class PinBase
    {
        public event Action<bool>? SignalChanged;
        public event Action<InputDevice?>? InputDeviceChanged;
        public event Action<FunctionConfig?>? FunctionConfigChanged;

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

        private FunctionConfig? _functionConfig;
        public FunctionConfig? FunctionConfig
        {
            get { return _functionConfig; }
            set
            {
                if (_functionConfig is not null)
                {
                    _functionConfig.ConfigChanged -= (s, e) => OnFunctionConfigChanged();
                }
                _functionConfig = value;

                if (_functionConfig is not null)
                {
                    _functionConfig.ConfigChanged += (s, e) => OnFunctionConfigChanged();
                }

                OnFunctionConfigChanged();
            }
        }

        protected PinBase(int number)
        {
            Number = number;
        }

        public abstract void UpdateSignal();

        private void OnFunctionConfigChanged()
        {
            if (FunctionConfig is not null &&
                FunctionConfig.Function is null)
            {
                FunctionConfig = null;
                return;
            }

            FunctionConfigChanged?.Invoke(FunctionConfig);
        }
    }
}
