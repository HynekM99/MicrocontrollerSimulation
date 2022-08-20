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

        public event Action? IntervalChanged;
        public event Action? StoppedRunning;
        public event Action? StartedRunning;

        private readonly Timer _timer = new();

        public override string Name { get { return NAME; } }

        public double Interval
        {
            get { return _timer.Interval; }
            set
            {
                double oldValue = _timer.Interval;
                _timer.Interval = value;
                if (Math.Abs(oldValue - value) > 0.0001)
                {
                    IntervalChanged?.Invoke();
                    OnDeviceEdited();
                }
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
                    Signal = false;
                    StoppedRunning?.Invoke();
                }
            }
        }

        public ClockDevice()
        {
            _timer.Elapsed += OnTimerElapsed;
        }

        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            Signal = !Signal;
        }

        public override string ToString()
        {
            return $"{NAME} [{Interval} ms]";
        }
    }
}
