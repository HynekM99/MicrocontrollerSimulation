using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Devices.Overviews
{
    public class SwitchDeviceOverviewViewModel : DeviceOverviewViewModel
    {
        private readonly SwitchDevice _switchDevice;

        public SwitchDeviceOverviewViewModel(SwitchDevice switchDevice)
        {
            _switchDevice = switchDevice;
        }
    }
}
