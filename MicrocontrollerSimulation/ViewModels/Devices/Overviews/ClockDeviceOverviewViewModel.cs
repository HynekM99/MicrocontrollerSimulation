using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Devices.Overviews
{
    public class ClockDeviceOverviewViewModel : DeviceOverviewViewModel
    {
        private readonly ClockDevice _clockDevice;

        public double Interval
        {
            get { return _clockDevice.Interval / 1000; }
        }

        public int? Signal { get; } = null;

        public ClockDeviceOverviewViewModel(ClockDevice clockDevice)
        {
            _clockDevice = clockDevice;

            _clockDevice.IntervalChanged += OnIntervalChanged;
        }

        private void OnIntervalChanged()
        {
            OnPropertyChanged(nameof(Interval));
        }

        public override void Dispose()
        {
            _clockDevice.IntervalChanged -= OnIntervalChanged;
            base.Dispose();
        }
    }
}
