using MicrocontrollerSimulation.Models.Microcontroller;
using MicrocontrollerSimulation.Models.Microcontroller.Pins;
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
            }
        }

        private PinBase? _selectedPin;
        public PinBase? SelectedPin
        {
            get { return _selectedPin; }
            private set
            {
                _selectedPin = value;
                OnPropertyChanged(nameof(SelectedPin));
            }
        }

        public PinsOverviewViewModel(Microcontroller microcontroller)
        {
            _microcontroller = microcontroller;
        }
    }
}
