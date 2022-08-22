using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Devices.Overviews;
using MicrocontrollerSimulation.ViewModels.Simulation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.NetworkInformation;
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
                Header = $"Konfigurace pinu {(SelectedPin is not null ? value : "(Vyberte pin)")}";
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

        public DeviceOverviewViewModel?[] DeviceOverviewViewModels { get; }

        public PinsOverviewViewModel(
            Microcontroller microcontroller,
            Func<SelectedPinConfigurationViewModel> createPinConfigurationViewModel)
        {
            _microcontroller = microcontroller;
            _createPinConfigurationViewModel = createPinConfigurationViewModel;

            SelectedPinConfigurationViewModel = _createPinConfigurationViewModel();
            SelectedPinConfigurationViewModel.OriginalPin = SelectedPin;

            var pins = _microcontroller.Pins;
            DeviceOverviewViewModels = new DeviceOverviewViewModel[30];

            UpdatePinViewModels();

            _microcontroller.ConfigurationChanged += OnConfigurationChanged;

            this.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnConfigurationChanged()
        {
            UpdatePinViewModels();
        }

        private void UpdatePinViewModels()
        {
            var pins = _microcontroller.Pins;
            for (int i = 0; i < pins.Length; i++)
            {
                DeviceOverviewViewModels[i] = GetDeviceViewModel(pins[i]);
            }
            OnPropertyChanged(nameof(DeviceOverviewViewModels));
        }

        private DeviceOverviewViewModel? GetDeviceViewModel(DigitalPin pin)
        {
            if (pin.PinMode == PinMode.Output)
            {
                if (pin.FunctionConfig is not null)
                {
                    return new OutputOverviewViewModel(pin);
                }
                return null;
            }

            if (pin.InputDevice is ButtonDevice btn)
            {
                return new ButtonDeviceOverviewViewModel(btn);
            }
            else if (pin.InputDevice is SwitchDevice sw)
            {
                return new SwitchDeviceOverviewViewModel(sw);
            }
            else if (pin.InputDevice is ClockDevice clk)
            {
                return new ClockDeviceOverviewViewModel(clk);
            }
            
            return null;
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectedPin))
            {
                SelectedPinConfigurationViewModel = _createPinConfigurationViewModel();
                SelectedPinConfigurationViewModel.OriginalPin = SelectedPin;
            }
        }

        public override void Dispose()
        {
            _microcontroller.ConfigurationChanged -= OnConfigurationChanged;
            foreach (var deviceVM in DeviceOverviewViewModels)
            {
                deviceVM?.Dispose();
            }
            base.Dispose();
        }
    }
}
