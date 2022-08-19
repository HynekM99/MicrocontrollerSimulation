using MicrocontrollerSimulation.Models.InputDevices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Simulation
{
    public class ClockDeviceViewModel : PinContentViewModel
    {
        private readonly ClockDevice _clockDevice;

        public int Signal
        {
            get { return _clockDevice.Signal ? 1 : 0; }
        }

        public bool IsRunning
        {
            get { return _clockDevice.IsRunning; }
            set { _clockDevice.IsRunning = value; }
        }

        public int Frequency
        {
            get { return (int)_clockDevice.Frequency; }
            set { _clockDevice.Frequency = value; }
        }

        public ClockDeviceViewModel(ClockDevice clockDevice)
        {
            _clockDevice = clockDevice;

            _clockDevice.FrequencyChanged += OnFrequencyChanged;
            _clockDevice.StartedRunning += OnRunningChanged;
            _clockDevice.StartedRunning += OnRunningChanged;
            _clockDevice.SignalChanged += OnSignalChanged;

            IsRunning = true;
        }

        private void OnSignalChanged(bool obj)
        {
            OnPropertyChanged(nameof(Signal));
        }

        private void OnRunningChanged()
        {
            OnPropertyChanged(nameof(IsRunning));
        }

        private void OnFrequencyChanged(double obj)
        {
            OnPropertyChanged(nameof(Frequency));
        }
    }
}
