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
        private readonly IDeviceFactory _deviceFactory;

        private DigitalPin? _originalPin;
        public DigitalPin? OriginalPin
        {
            get { return _originalPin; }
            set
            {
                _originalPin = value;
                RestoreConfiguration();
                OnPropertyChanged(nameof(OriginalPin));
            }
        }

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

        private int _clockInterval = 100;
        public int ClockInterval
        {
            get { return _clockInterval; }
            set
            {
                _clockInterval = value;
                OnPropertyChanged(nameof(ClockInterval));
            }
        }

        public List<string> AvailableDevices { get; }

        public SelectedPinInputModeConfigViewModel(IDeviceFactory deviceFactory)
        {
            _deviceFactory = deviceFactory;

            AvailableDevices = deviceFactory.GetAvailableDevices();
            AvailableDevices.Insert(0, "Žádné");

            SelectedDeviceName = AvailableDevices[0];

            PropertyChanged += OnViewModelPropertyChanged;
        }

        public void RestoreConfiguration()
        {
            SelectedDeviceName = _originalPin?.InputDevice?.Name;
            if (_originalPin?.InputDevice is ClockDevice clk)
            {
                ClockInterval = clk.Interval;
            }
        }

        public void SaveConfiguration()
        {
            _originalPin!.InputDevice = SelectedDevice;
            if (SelectedDevice is ClockDevice clk)
            {
                clk.Interval = ClockInterval;
            }
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

            if (SelectedDevice is ClockDevice &&
                ClockInterval != ((ClockDevice)_originalPin!.InputDevice!).Interval)
            {
                IsConfigDifferent = true;
                return;
            }

            IsConfigDifferent = false;
        }
    }
}
