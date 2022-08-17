﻿using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration
{
    public class FunctionConfig : IDisposable
    {
        public event EventHandler<ConfigEntry?>? ConfigChanged;

        private readonly FunctionsCollection _functions;

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

        public ReadOnlyCollection<ConfigEntry>? ConfigEntries { get; private set; }

        public FunctionConfig(string functionName, FunctionsCollection functions)
        {
            var function = functions.
                Where(f => f.Name == functionName).
                FirstOrDefault()
                ?? throw new ArgumentException($"Function called \"{functionName}\" does not exist.");

            _functions = functions;
            Function = function;

            var entries = new List<ConfigEntry>();
            foreach (var input in function.Expression.Inputs)
            {
                var entry = new ConfigEntry(input);
                entry.PinNumberChanged += OnEntryPinNumberChanged;
                entries.Add(entry);
            }
            ConfigEntries = new(entries);

            functions.CollectionChanged += OnAvailableFunctionsChanged;
        }

        private void OnAvailableFunctionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_functions.Any(f => f.Name == Function!.Name))
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
            ConfigEntries = null;
            ConfigEntries?.ToList().ForEach(e => e.PinNumberChanged -= OnEntryPinNumberChanged);
            GC.SuppressFinalize(this);
        }

        public override bool Equals(object? obj)
        {
            if (obj is FunctionConfig config)
            {
                if (Function is null && config.Function is null)
                {
                    return true;
                }

                if (ConfigEntries!.Count != config.ConfigEntries!.Count)
                {
                    return false;
                }

                for (int i = 0; i < ConfigEntries!.Count; i++)
                {
                    var entry = ConfigEntries[i];
                    var objEntry = config.ConfigEntries![i];

                    if (!entry.Equals(objEntry))
                    {
                        return false;
                    }
                }

                return Function is not null && config.Function is not null &&
                       Function.Name == config.Function.Name;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Function, ConfigEntries);
        }
    }
}
