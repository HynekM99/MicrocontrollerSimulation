using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Devices.Configs
{
    public class ClockDeviceConfigViewModel : ViewModelBase
    {
        private readonly ClockDevice _clockDevice;

        public double Interval
        {
            get { return _clockDevice.Interval / 1000; }
            set
            {
                try
                {
                    _clockDevice.Interval = value * 1000;
                }
                catch
                {
                    _clockDevice.Interval = MinValue;
                }
            }
        }

        public double MinValue
        {
            get { return 0.1; }
        }

        public ClockDeviceConfigViewModel(ClockDevice clockDevice)
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
