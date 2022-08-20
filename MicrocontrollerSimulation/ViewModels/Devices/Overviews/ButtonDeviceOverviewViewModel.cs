using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Devices.Overviews
{
    public class ButtonDeviceOverviewViewModel : DeviceOverviewViewModel
    {
        private readonly ButtonDevice _buttonDevice;

        public ButtonDeviceOverviewViewModel(ButtonDevice buttonDevice)
        {
            _buttonDevice = buttonDevice;
        }
    }
}
