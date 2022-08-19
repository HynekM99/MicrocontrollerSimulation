using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MicrocontrollerSimulation.Models.Microcontrollers
{
    public class Microcontroller : IDisposable
    {
        private readonly Timer _timer = new();

        public event Action? ConfigurationChanged;
        public event Action? StateUpdated;
        public event Action? StoppedRunning;
        public event Action? StartedRunning;

        public DigitalPin[] Pins { get; } = new DigitalPin[30];

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

        public Microcontroller()
        {
            for (int i = 0; i < Pins.Length; i++)
            {
                Pins[i] = new DigitalPin(i, Pins);
                Pins[i].PinModeChanged += PinsPinModeChanged;
                Pins[i].InputDeviceChanged += PinsInputDeviceChanged;
                Pins[i].FunctionConfigChanged += PinsFunctionConfigChanged;
            }

            _timer.Elapsed += UpdatePins;
            _timer.Interval = 1;
        }

        private void PinsPinModeChanged()
        {
            ConfigurationChanged?.Invoke();
        }

        private void PinsInputDeviceChanged()
        {
            ConfigurationChanged?.Invoke();
        }

        private void PinsFunctionConfigChanged()
        {
            ConfigurationChanged?.Invoke();
        }

        private void UpdatePins(object? sender, ElapsedEventArgs e)
        {
            foreach (var pin in Pins)
            {
                pin.UpdateSignal();
            }
            StateUpdated?.Invoke();
        }

        public void Dispose()
        {
            foreach (var pin in Pins)
            {
                pin.PinModeChanged -= PinsPinModeChanged;
                pin.InputDeviceChanged -= PinsInputDeviceChanged;
                pin.FunctionConfigChanged -= PinsFunctionConfigChanged;
            }
            GC.SuppressFinalize(this);
        }
    }
}
