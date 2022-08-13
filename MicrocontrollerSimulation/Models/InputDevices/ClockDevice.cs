using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MicrocontrollerSimulation.Models.InputDevices
{
    public class ClockDevice : InputDevice
    {
        public const string NAME = "Hodinový signál";

        public event Action<double>? FrequencyChanged;

        private Timer _timer = new Timer();

        public override string Name { get { return NAME; } }

        private double _frequency;
        public double Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                double interval = 500/value;
                _timer.Interval = interval;
                FrequencyChanged?.Invoke(value);
            }
        }

        public ClockDevice()
        {
            _timer.Elapsed += (s, e) => Signal = !Signal;
            _timer.Start();
        }

        public ClockDevice(double frequency) : this()
        {
            Frequency = frequency;
        }

        public override string ToString()
        {
            return $"{NAME} [{Frequency:00} Hz]";
        }
    }
}
