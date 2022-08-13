﻿using MicrocontrollerSimulation.Models.Functions.Provider;
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
    public class SelectedPinConfigurationViewModel : ViewModelBase
    {
        private readonly DigitalPin? _originalPin;

        public DigitalPin? OriginalPin => _originalPin;

        private PinMode _selectedPinMode;
        public PinMode SelectedPinMode
        {
            get { return _selectedPinMode; }
            set
            {
                _selectedPinMode = value;
                OnPropertyChanged(nameof(SelectedPinMode));
                CurrentConfigViewModel = value == PinMode.Input ?
                    SelectedPinInputModeConfigViewModel :
                    SelectedPinOutputModeConfigViewModel;
            }
        }

        public SelectedPinInputModeConfigViewModel SelectedPinInputModeConfigViewModel { get; }
        public SelectedPinOutputModeConfigViewModel SelectedPinOutputModeConfigViewModel { get; }

        private ViewModelBase _currentConfigViewModel;
        public ViewModelBase CurrentConfigViewModel
        {
            get { return _currentConfigViewModel; }
            set
            {
                _currentConfigViewModel = value;
                OnPropertyChanged(nameof(CurrentConfigViewModel));
            }
        }

        public SelectedPinConfigurationViewModel(
            DigitalPin? originalPin,
            SelectedPinInputModeConfigViewModel selectedPinInputModeConfigViewModel,
            SelectedPinOutputModeConfigViewModel selectedPinOutputModeConfigViewModel)
        {
            _originalPin = originalPin;

            SelectedPinInputModeConfigViewModel = selectedPinInputModeConfigViewModel;
            SelectedPinOutputModeConfigViewModel = selectedPinOutputModeConfigViewModel;
            _currentConfigViewModel = selectedPinInputModeConfigViewModel;

            SelectedPinMode = _originalPin is null ? PinMode.Input : _originalPin.PinMode;
        }
    }
}
