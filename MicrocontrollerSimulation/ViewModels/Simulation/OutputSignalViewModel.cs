﻿using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Simulation
{
    public class OutputSignalViewModel : PinContentViewModel
    {
        private readonly DigitalPin _pin;

        public int Signal
        {
            get { return _pin.Signal ? 1 : 0; }
        }

        public OutputSignalViewModel(DigitalPin pin)
        {
            _pin = pin;

            _pin.SignalChanged += OnSignalChanged;
        }

        private void OnSignalChanged()
        {
            OnPropertyChanged(nameof(Signal));
        }
    }
}
