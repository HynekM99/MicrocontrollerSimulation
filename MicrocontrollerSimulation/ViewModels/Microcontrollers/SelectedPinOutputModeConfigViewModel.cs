using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinOutputModeConfigViewModel : ViewModelBase
    {
        private readonly FunctionsCollection _functions;

        private DigitalPin? _originalPin;
        public DigitalPin? OriginalPin
        {
            get { return _originalPin; }
            set
            {
                _originalPin = value;
                UpdateSearchResults();
                RestoreConfiguration();
                OnPropertyChanged(nameof(OriginalPin));
            }
        }

        private string? _searchedFunctionName;
        public string? SearchedFunctionName
        {
            get { return _searchedFunctionName; }
            set
            {
                _searchedFunctionName = value;
                OnPropertyChanged(nameof(SearchedFunctionName));
                UpdateSearchResults();
            }
        }

        private List<string>? _searchResults;
        public List<string>? SearchResults
        {
            get { return _searchResults; }
            set
            {
                _searchResults = value;
                OnPropertyChanged(nameof(SearchResults));
            }
        }

        private string? _selectedFunctionName;
        public string? SelectedFunctionName
        {
            get { return _selectedFunctionName; }
            set
            {
                _selectedFunctionName = value;
                OnPropertyChanged(nameof(SelectedFunctionName));
            }
        }

        public Function? Function
        {
            get { return _functions.Where(f => f.Name == SelectedFunctionName).FirstOrDefault(); }
        }

        public List<string> AvailableFunctions
        {
            get
            {
                List<string> availableFunctions = new();
                foreach (var function in _functions)
                {
                    availableFunctions.Add(function.Name);
                }
                return availableFunctions;
            }
        }

        private FunctionConfig? _functionConfig;
        public FunctionConfig? FunctionConfig
        {
            get { return _functionConfig; }
            set
            {
                if (_functionConfig is not null)
                {
                    _functionConfig.ConfigChanged -= OnFunctionConfigChanged;
                }

                _functionConfig = value;

                if (_functionConfig is not null)
                {
                    _functionConfig.ConfigChanged += OnFunctionConfigChanged;
                }

                OnPropertyChanged(nameof(FunctionConfig));
            }
        }

        public SelectedPinOutputModeConfigViewModel(FunctionsCollection functions)
        {
            _functions = functions;

            _functions.CollectionChanged += OnAvailableFunctionsChanged;
            PropertyChanged += OnViewModelPropertyChanged;
        }

        public void RestoreConfiguration()
        {
            SelectedFunctionName = _originalPin?.FunctionConfig?.Function?.Name;
        }

        private void ResetFunctionConfigEntries()
        {
            foreach (var originalEntry in _originalPin!.FunctionConfig!.ConfigEntries!)
            {
                var entry = FunctionConfig!.ConfigEntries!.
                    Where(e => e.Input.AsString == originalEntry.Input.AsString).
                    FirstOrDefault();

                entry!.PinNumber = originalEntry.PinNumber;
            }
        }

        private void OnAvailableFunctionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(AvailableFunctions));
            UpdateSearchResults();
        }

        private void UpdateSearchResults()
        {
            if (string.IsNullOrWhiteSpace(SearchedFunctionName))
            {
                SearchResults = AvailableFunctions;
                return;
            }

            SearchResults = AvailableFunctions.
                Where(f =>
                f.Contains(SearchedFunctionName, StringComparison.CurrentCultureIgnoreCase)).
                ToList();
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_originalPin is null)
            {
                return;
            }

            if (e.PropertyName == nameof(SelectedFunctionName))
            {
                if (!_functions.Any(f => f.Name == SelectedFunctionName))
                {
                    FunctionConfig = null;
                    return;
                }

                FunctionConfig = new(SelectedFunctionName!, _functions);

                if ( _originalPin!.FunctionConfig is not null &&
                    SelectedFunctionName == _originalPin!.FunctionConfig!.Function!.Name)
                {
                    ResetFunctionConfigEntries();
                }
            }
        }

        private void OnFunctionConfigChanged(object? sender, ConfigEntry? e)
        {
            OnPropertyChanged(nameof(FunctionConfig));
        }

        public override void Dispose()
        {
            _functions.CollectionChanged -= OnAvailableFunctionsChanged;
            base.Dispose();
        }
    }
}
