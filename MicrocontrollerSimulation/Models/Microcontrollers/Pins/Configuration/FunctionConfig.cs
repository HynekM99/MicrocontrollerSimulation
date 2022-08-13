using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Provider;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration
{
    public class FunctionConfig : IDisposable
    {
        public event EventHandler<ConfigEntry?>? ConfigChanged;

        private readonly IFunctionsProvider _functionsProvider;

        private Function? _function;
        public Function? Function
        {
            get { return _function; }
            private set
            {
                _function = value;
                ConfigChanged?.Invoke(this, null);
            }
        }
        public ReadOnlyCollection<PinBase>? Pins { get; set; }
        public ReadOnlyCollection<ConfigEntry>? ConfigEntries { get; private set; }

        public FunctionConfig(string functionName, IFunctionsProvider functionsProvider)
        {
            var function = functionsProvider.Request(functionName)
                ?? throw new ArgumentException($"Function called \"{functionName}\" does not exist.");

            _functionsProvider = functionsProvider;
            Function = function;

            var entries = new List<ConfigEntry>();
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
            if (!_functionsProvider.CanProvide(Function!.Name))
            {
                Dispose();
            }            
        }

        private void OnEntryPinNumberChanged(ConfigEntry entry)
        {
            ConfigChanged?.Invoke(this, entry);
        }

        public void Dispose()
        {
            Function = null;
            Pins = null;
            ConfigEntries = null;
            ConfigEntries?.ToList().ForEach(e => e.PinNumberChanged -= OnEntryPinNumberChanged);
            GC.SuppressFinalize(this);
        }
    }
}
