﻿using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MicrocontrollerSimulation.Models.Microcontrollers
{
    public class Microcontroller
    {
        private readonly Timer _timer = new();

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
            }

            _timer.Elapsed += UpdatePins;
            _timer.Interval = 1;
        }

        private void UpdatePins(object? sender, ElapsedEventArgs e)
        {
            foreach (var pin in Pins)
            {
                pin.UpdateSignal();
            }
            StateUpdated?.Invoke();
        }
    }
}
