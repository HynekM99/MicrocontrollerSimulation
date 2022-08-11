﻿using MicrocontrollerSimulation.Models.Microcontroller.Pins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MicrocontrollerSimulation.Models.Microcontroller
{
    public class Microcontroller
    {
        private readonly Timer _timer = new();

        public event Action? StateUpdated;

        public PinBase[] Pins { get; } = new PinBase[30];

        public Microcontroller()
        {
            for (int i = 0; i < Pins.Length; i++)
            {
                Pins[i] = new InputPin(i);
            }

            _timer.Elapsed += UpdatePins;
            _timer.Interval = 1;
        }

        private void UpdatePins(object? sender, ElapsedEventArgs e)
        {
            StateUpdated?.Invoke();
        }
    }
}
