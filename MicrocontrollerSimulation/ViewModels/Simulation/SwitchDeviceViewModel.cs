using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Simulation
{
    public class SwitchDeviceViewModel : PinContentViewModel
    {
        private readonly SwitchDevice _switchDevice;

        public bool IsToggled
        {
            get { return _switchDevice.IsToggled; }
            set { _switchDevice.IsToggled = value; }
        }

        public SwitchDeviceViewModel(SwitchDevice switchDevice)
        {
            _switchDevice = switchDevice;

            _switchDevice.SignalChanged += OnSignalChanged;
        }

        private void OnSignalChanged(bool obj)
        {
            OnPropertyChanged(nameof(IsToggled));
        }
    }
}
