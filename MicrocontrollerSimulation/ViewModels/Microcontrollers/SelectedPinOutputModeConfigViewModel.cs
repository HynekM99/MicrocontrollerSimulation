using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins.Configuration;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Microcontrollers
{
    public class SelectedPinOutputModeConfigViewModel : ViewModelBase
    {
        private readonly FunctionsCollection _functions;

        private DigitalPin? _originalPin;
        public DigitalPin? OriginalPin
        {
            get { return _originalPin; }
            set
            {
                _originalPin = value;

                if (_originalPin is not null)
                {
                    _originalPin.FunctionConfigChanged += OnFunctionConfigChanged;
                }

                OnPropertyChanged(nameof(SelectedFunction));
                OnPropertyChanged(nameof(FunctionConfig));
                OnPropertyChanged(nameof(OriginalPin));
            }
        }

        public FunctionsCollection Functions => _functions;

        public Function? SelectedFunction
        {
            get { return FunctionConfig?.Function; }
            set
            {
                if (!_functions.Any(f => f == value))
                {
                    FunctionConfig = null;
                    return;
                }

                FunctionConfig = new(value!.Name, _functions);

                OnPropertyChanged(nameof(SelectedFunction));
            }
        }

        public FunctionConfig? FunctionConfig
        {
            get { return _originalPin?.FunctionConfig; }
            set
            {
                if (_originalPin is not null)
                {
                    _originalPin!.FunctionConfig = value;
                }
            }
        }

        public SelectedPinOutputModeConfigViewModel(FunctionsCollection functions)
        {
            _functions = functions;

            _functions.CollectionChanged += OnFunctionsCollectionChanged;
        }

        private void OnFunctionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(Functions));
            OnPropertyChanged(nameof(SelectedFunction));
        }

        private void OnFunctionConfigChanged()
        {
            OnPropertyChanged(nameof(FunctionConfig));
            OnPropertyChanged(nameof(SelectedFunction));
        }

        public override void Dispose()
        {
            if (_originalPin is not null)
            {
                _originalPin.FunctionConfigChanged -= OnFunctionConfigChanged;
            }

            _functions.CollectionChanged -= OnFunctionsCollectionChanged;
            base.Dispose();
        }
    }
}
