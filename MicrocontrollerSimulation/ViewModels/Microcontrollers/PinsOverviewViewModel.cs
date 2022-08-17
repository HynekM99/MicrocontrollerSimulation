using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class PinsOverviewViewModel : ViewModelBase
    {
        private readonly Microcontroller _microcontroller;
        private readonly Func<SelectedPinConfigurationViewModel> _createPinConfigurationViewModel;

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

        private SelectedPinConfigurationViewModel? _selectedPinConfigurationViewModel;
        public SelectedPinConfigurationViewModel? SelectedPinConfigurationViewModel
        {
            get { return _selectedPinConfigurationViewModel; }
            set
            {
                _selectedPinConfigurationViewModel = value;
                OnPropertyChanged(nameof(SelectedPinConfigurationViewModel));
            }
        }

        public PinsOverviewViewModel(
            Microcontroller microcontroller,
            Func<SelectedPinConfigurationViewModel> createPinConfigurationViewModel)
        {
            _microcontroller = microcontroller;
            _createPinConfigurationViewModel = createPinConfigurationViewModel;

            SelectedPinConfigurationViewModel = _createPinConfigurationViewModel();
            SelectedPinConfigurationViewModel.OriginalPin = SelectedPin;

            this.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPin))
            {
                SelectedPinConfigurationViewModel = _createPinConfigurationViewModel();
                SelectedPinConfigurationViewModel.OriginalPin = SelectedPin;
            }
        }
    }
}
