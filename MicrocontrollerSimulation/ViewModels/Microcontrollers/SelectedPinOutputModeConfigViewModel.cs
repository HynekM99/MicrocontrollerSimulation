using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Provider;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinOutputModeConfigViewModel : ViewModelBase
    {
        private readonly DigitalPin? _originalPin;
        private readonly IFunctionsProvider _functionsProvider;

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
            get { return _functionsProvider.Request(SelectedFunctionName); }
        }

        public List<string> AvailableFunctions
        {
            get { return _functionsProvider.GetAvailableFunctions(); }
        }

        private FunctionConfig? _functionConfig;
        public FunctionConfig? FunctionConfig
        {
            get { return _functionConfig; }
            set
            {
                _functionConfig = value;
                OnPropertyChanged(nameof(FunctionConfig));
            }
        }

        public SelectedPinOutputModeConfigViewModel(
            DigitalPin? originalPin,
            IFunctionsProvider functionsProvider)
        {
            _originalPin = originalPin;
            _functionsProvider = functionsProvider;

            _functionsProvider.AvailableFunctionsChanged += OnAvailableFunctionsChanged;
            PropertyChanged += OnViewModelPropertyChanged;

            UpdateSearchResults();
            Reset();
        }

        public void Reset()
        {
            SelectedFunctionName = _originalPin?.FunctionConfig?.Function?.Name;
            FunctionConfig = _originalPin?.FunctionConfig;
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

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedFunctionName))
            {
                if (!_functionsProvider.CanProvide(SelectedFunctionName))
                {
                    FunctionConfig = null;
                    return;
                }

                FunctionConfig = new(SelectedFunctionName!, _functionsProvider);

                if (SelectedFunctionName == _originalPin!.FunctionConfig!.Function!.Name)
                {
                    ResetFunctionConfigEntries();
                }
            }
        }

        private void OnAvailableFunctionsChanged()
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
    }
}
