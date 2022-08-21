using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration
{
    public class ConfigEntries : ReadOnlyObservableCollection<ConfigEntry>
    {
        public ConfigEntries(ObservableCollection<ConfigEntry> entries) : base(entries)
        {
            foreach (var entry in entries)
            {
                entry.EntryChanged += OnEntryChanged;
            }
        }

        private void OnEntryChanged()
        {
            OnCollectionChanged(new(NotifyCollectionChangedAction.Reset));
        }
    }
}
