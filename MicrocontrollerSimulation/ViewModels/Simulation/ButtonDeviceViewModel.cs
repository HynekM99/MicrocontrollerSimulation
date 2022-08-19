using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Simulation
{
    public class ButtonDeviceViewModel : PinContentViewModel
    {
        private readonly ButtonDevice _buttonDevice;

        public bool IsPressed
        {
            get { return _buttonDevice.Pressed; }
            set { _buttonDevice.Pressed = value; }
        }

        public ButtonDeviceViewModel(ButtonDevice buttonDevice)
        {
            _buttonDevice = buttonDevice;

            _buttonDevice.SignalChanged += OnSignalChanged;
        }

        private void OnSignalChanged(bool obj)
        {
            OnPropertyChanged(nameof(IsPressed));
        }
    }
}
