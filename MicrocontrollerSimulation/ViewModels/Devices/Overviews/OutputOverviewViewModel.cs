using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Devices.Overviews
{
    public class OutputOverviewViewModel : DeviceOverviewViewModel
    {
        private readonly DigitalPin _pin;

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

        public OutputOverviewViewModel(DigitalPin pin)
        {
            _pin = pin;

            pin.FunctionConfigChanged += OnFunctionConfigChanged;
        }

        private void OnFunctionConfigChanged()
        {
            OnPropertyChanged(nameof(ToolTip));
        }

        public override void Dispose()
        {
            _pin.FunctionConfigChanged -= OnFunctionConfigChanged;
            base.Dispose();
        }
    }
}
