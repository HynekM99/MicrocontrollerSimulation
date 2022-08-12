using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Provider;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontroller.Pins.Configuration
{
    public class FunctionConfig : IDisposable
    {
        public event EventHandler<ConfigEntry>? ConfigChanged;

        private readonly IFunctionsProvider _functionsProvider;

        public string? FunctionName { get; private set; }
        public ReadOnlyCollection<PinBase>? Pins { get; set; }
        public ReadOnlyCollection<ConfigEntry>? ConfigEntries { get; private set; }

        public FunctionConfig(string functionName, IFunctionsProvider functionsProvider)
        {
            FunctionName = functionName;
            _functionsProvider = functionsProvider;

            var function = functionsProvider.Request(functionName)
                ?? throw new ArgumentException($"Function called \"{functionName}\" does not exist.");

            var entries = new ObservableCollection<ConfigEntry>();

            foreach (var input in function.Expression.Inputs)
            {
                var entry = new ConfigEntry(input);
                entry.PinNumberChanged += OnEntryPinNumberChanged;
                entries.Add(entry);
            }

            ConfigEntries = new(entries);

            functionsProvider.AvailableFunctionsChanged += OnAvailableFunctionsChanged;
        }

        private void OnAvailableFunctionsChanged()
        {
            var function = _functionsProvider.Request(FunctionName!);

            if (function is not null) return;

            FunctionName = null;
            ConfigEntries = null;
        }

        private void OnEntryPinNumberChanged(ConfigEntry entry)
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
