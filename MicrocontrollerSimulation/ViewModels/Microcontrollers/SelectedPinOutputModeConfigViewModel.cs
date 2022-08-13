using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Provider;
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

        public SelectedPinOutputModeConfigViewModel(IFunctionsProvider functionsProvider)
        {
            _functionsProvider = functionsProvider;

            UpdateSearchResults();
            _functionsProvider.AvailableFunctionsChanged += OnAvailableFunctionsChanged;
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
