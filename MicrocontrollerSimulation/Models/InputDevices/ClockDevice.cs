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
        public event Action? StoppedRunning;
        public event Action? StartedRunning;

        private Timer _timer = new();

        public override string Name { get { return NAME; } }

        private double _frequency = 1;
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

        public bool IsRunning
        {
            get { return _timer.Enabled; }
            set
            {
                if (value)
                {
                    _timer.Start();
                    StartedRunning?.Invoke();
                }
                else
                {
                    _timer.Stop();
                    StoppedRunning?.Invoke();
                }
            }
        }

        public ClockDevice()
        {
            _timer.Elapsed += OnTimerElapsed;

            Frequency = 1;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Signal = !Signal;
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
