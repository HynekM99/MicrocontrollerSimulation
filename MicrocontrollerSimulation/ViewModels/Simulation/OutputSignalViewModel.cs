using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
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

        public string ToolTip
        {
            get
            {
                string tooltip = _pin.FunctionConfig!.Function!.Name;
                tooltip += Environment.NewLine;

                foreach (var entry in _pin.FunctionConfig!.ConfigEntries!)
                {
                    tooltip += $"{entry.Input}...{entry.PinNumber}";
                    tooltip += Environment.NewLine;
                }

                return tooltip;
            }
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

        public override void Dispose()
        {
            _pin.SignalChanged -= OnSignalChanged;
            base.Dispose();
        }
    }
}
