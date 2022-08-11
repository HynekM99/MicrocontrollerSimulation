using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontroller.Pins.PinConfiguration
{
    public class OutputFunctionInputPinsConfig : IDisposable
    {
        public event EventHandler<ExpressionInputPinConfigEntry>? ConfigChanged;

        public Function Function { get; }
        public ReadOnlyCollection<PinBase>? Pins { get; set; }
        public ReadOnlyCollection<ExpressionInputPinConfigEntry> ConfigEntries { get; }

        public OutputFunctionInputPinsConfig(Function function)
        {
            Function = function;

            var entries = new ObservableCollection<ExpressionInputPinConfigEntry>();

            foreach (var input in function.Expression.Inputs)
            {
                var entry = new ExpressionInputPinConfigEntry(input);
                entry.PinNumberChanged += OnEntryPinNumberChanged;
                entries.Add(entry);
            }

            ConfigEntries = new(entries);
        }

        private void OnEntryPinNumberChanged(ExpressionInputPinConfigEntry entry)
        {
            ConfigChanged?.Invoke(this, entry);
        }

        public void Dispose()
        {
            foreach (var entry in ConfigEntries)
            {
                entry.PinNumberChanged -= OnEntryPinNumberChanged;
            }
            GC.SuppressFinalize(this);
        }
    }
}
