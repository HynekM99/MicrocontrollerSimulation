using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class PinsOverviewViewModel : ViewModelBase
    {
        private readonly Microcontroller _microcontroller;
        private readonly NavigationStore<PinsOverviewViewModel> _navigationStore;
        private readonly NavigationService<PinsOverviewViewModel, SelectedPinConfigurationViewModel> _navigationService;

        private string _header = "Konfigurace pinu (Vyberte pin)";
        public string Header
        {
            get { return _header; }
            set
            {
                _header = value;
                OnPropertyChanged(nameof(Header));
            }
        }

        private int _selectedPinNumber = -1;
        public int SelectedPinNumber
        {
            get { return _selectedPinNumber; }
            set
            {
                _selectedPinNumber = value;
                SelectedPin = _microcontroller.Pins.
                    Where(p => p.Number == value).
                    FirstOrDefault();
                OnPropertyChanged(nameof(SelectedPinNumber));
                Header = $"Konfigurace pinu {(value > -1 ? value : "(Vyberte pin)")}";
            }
        }

        private DigitalPin? _selectedPin;
        public DigitalPin? SelectedPin
        {
            get { return _selectedPin; }
            private set
            {
                _selectedPin = value;
                OnPropertyChanged(nameof(SelectedPin));
            }
        }

        public ViewModelBase? SelectedPinConfigurationViewModel => _navigationStore.CurrentViewModel;

        public PinsOverviewViewModel(
            Microcontroller microcontroller,
            NavigationStore<PinsOverviewViewModel> navigationStore,
            NavigationService<PinsOverviewViewModel, SelectedPinConfigurationViewModel> navigationService)
        {
            _microcontroller = microcontroller;
            _navigationStore = navigationStore;
            _navigationService = navigationService;

            this.PropertyChanged += OnViewModelPropertyChanged;
            navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(SelectedPinConfigurationViewModel));
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPin))
            {
                _navigationService.Navigate();
            }
        }
    }
}
