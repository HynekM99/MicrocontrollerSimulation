using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.InputDevices;
using MicrocontrollerSimulation.Models.InputDevices.Factories;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinInputModeConfigViewModel : ViewModelBase
    {
        private DigitalPin? _originalPin;
        public DigitalPin? OriginalPin
        {
            get { return _originalPin; }
            set
            {
                _originalPin = value;

                if (_originalPin is not null)
                {
                    _originalPin.InputDeviceChanged += OnInputDeviceChanged;

                    var device = _originalPin.InputDevice;

                    if (device is not null)
                    {
                        var equivalentDevice = AvailableDevices
                        .Where(d => d.GetType() == device.GetType())
                        .First();
                        int index = AvailableDevices.IndexOf(equivalentDevice);
                        AvailableDevices.Remove(equivalentDevice);
                        AvailableDevices.Insert(index, device);
                    }

                    OnPropertyChanged(nameof(SelectedDevice));
                    OnPropertyChanged(nameof(DeviceConfigViewModel));
                }

                OnPropertyChanged(nameof(OriginalPin));
            }
        }

        public InputDevice? SelectedDevice
        {
            get { return _originalPin is null ? null : _originalPin.InputDevice; }
            set { if (_originalPin is not null) _originalPin.InputDevice = value; }
        }

        public ViewModelBase? DeviceConfigViewModel
        {
            get { return GetDeviceConfigViewModel(SelectedDevice); }
        }

        public List<InputDevice> AvailableDevices { get; }

        public System.Windows.Input.ICommand ResetDevice { get; }

        public SelectedPinInputModeConfigViewModel(IDeviceFactory deviceFactory)
        {
            AvailableDevices = deviceFactory.CreateAllDevices();

            ResetDevice = new RelayCommand(e => SelectedDevice = null);
        }

        private ViewModelBase? GetDeviceConfigViewModel(InputDevice? device)
        {
            if (SelectedDevice is ClockDevice clk)
            {
                return new ClockDeviceConfigViewModel(clk);
            }
            return null;
        }

        private void OnInputDeviceChanged()
        {
            OnPropertyChanged(nameof(SelectedDevice));
            OnPropertyChanged(nameof(DeviceConfigViewModel));
        }

        public override void Dispose()
        {
            if (_originalPin is not null)
            {
                _originalPin.InputDeviceChanged -= OnInputDeviceChanged;
            }

            base.Dispose();
        }
    }
}
