using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.InputDevices.Factories;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinInputModeConfigViewModel : ViewModelBase
    {
        private readonly DigitalPin? _originalPin;
        private readonly IDeviceFactory _deviceFactory;

        private bool _isConfigDifferent = false;
        public bool IsConfigDifferent
        {
            get { return _isConfigDifferent; }
            set
            {
                _isConfigDifferent = value;
                OnPropertyChanged(nameof(IsConfigDifferent));
            }
        }

        private string? _selectedDeviceName;
        public string? SelectedDeviceName
        {
            get { return _selectedDeviceName; }
            set
            {
                _selectedDeviceName = value;

                if (value is null) _selectedDeviceName = "Žádné";

                OnPropertyChanged(nameof(SelectedDeviceName));
                SelectedDevice = _deviceFactory.CreateDevice(value);
            }
        }

        private InputDevice? _selectedDevice;
        public InputDevice? SelectedDevice
        {
            get { return _selectedDevice; }
            set
            {
                _selectedDevice = value;
                OnPropertyChanged(nameof(SelectedDevice));
                ClockConfigVisible = SelectedDevice is ClockDevice;
            }
        }

        private bool _clockConfigVisible = false;
        public bool ClockConfigVisible
        {
            get { return _clockConfigVisible; }
            set
            {
                _clockConfigVisible = value;
                OnPropertyChanged(nameof(ClockConfigVisible));
            }
        }

        private int _clockFrequency = 1;
        public int ClockFrequency
        {
            get { return _clockFrequency; }
            set
            {
                _clockFrequency = value;
                OnPropertyChanged(nameof(ClockFrequency));
            }
        }

        public List<string> AvailableDevices { get; }

        public SelectedPinInputModeConfigViewModel(
            DigitalPin? originalPin,
            IDeviceFactory deviceFactory)
        {
            _originalPin = originalPin;
            _deviceFactory = deviceFactory;

            AvailableDevices = deviceFactory.GetAvailableDevices();
            AvailableDevices.Insert(0, "Žádné");

            SelectedDeviceName = AvailableDevices[0];

            PropertyChanged += OnViewModelPropertyChanged;

            RestoreConfiguration();
        }

        public void RestoreConfiguration()
        {
            SelectedDeviceName = _originalPin?.InputDevice?.Name;
            if (_originalPin?.InputDevice is ClockDevice clk)
            {
                ClockFrequency = (int)clk.Frequency;
            }
        }

        public void SaveConfiguration()
        {
            if (SelectedDevice is ClockDevice clk)
            {
                _originalPin!.InputDevice = clk;
                clk.Frequency = ClockFrequency;
                return;
            }

            _originalPin!.InputDevice = SelectedDevice;
            IsConfigDifferent = false;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (_originalPin is null || e.PropertyName == nameof(IsConfigDifferent))
            {
                return;
            }

            if ((SelectedDevice is null && _originalPin!.InputDevice is not null) ||
                (SelectedDevice is not null && _originalPin!.InputDevice is null))
            {
                IsConfigDifferent = true;
                return;
            }

            if (SelectedDevice is not null &&
                _originalPin!.InputDevice!.Name != SelectedDevice.Name)
            {
                IsConfigDifferent = true;
                return;
            }

            if (SelectedDevice is ClockDevice clk &&
                clk.Frequency != ((ClockDevice)_originalPin!.InputDevice!).Frequency)
            {
                IsConfigDifferent = true;
                return;
            }

            IsConfigDifferent = false;
        }
    }
}
