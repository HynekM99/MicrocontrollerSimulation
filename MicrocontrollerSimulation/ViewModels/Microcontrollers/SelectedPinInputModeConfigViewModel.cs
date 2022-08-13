using MicrocontrollerSimulation.Models.InputDevices.Factories;
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
        private string? _selectedDeviceType;
        public string? SelectedDeviceType
        {
            get { return _selectedDeviceType; }
            set
            {
                _selectedDeviceType = value;
                OnPropertyChanged(nameof(SelectedDeviceType));
            }
        }

        public List<string> AvailableDevices { get; }

        public SelectedPinInputModeConfigViewModel(IDeviceFactory deviceFactory)
        {
            AvailableDevices = deviceFactory.GetAvailableDevices();
        }
    }
}
