using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class CreateMultiFunctionViewModel<T> : ViewModelBase where T : LogicalExpression
    {
        private List<TemporaryFunctionViewModel>? _selectedFunctionViewModels;
        public List<TemporaryFunctionViewModel>? SelectedFunctionViewModels
        {
            get { return _selectedFunctionViewModels; }
            set
            {
                _selectedFunctionViewModels = value;
                OnPropertyChanged(nameof(SelectedFunctionViewModels));
            }
        }

        private FunctionsCollection? _temporaryFunctions;
        public FunctionsCollection? TemporaryFunctions
        {
            get { return _temporaryFunctions; }
            set
            {
                if (_temporaryFunctions is not null)
                {
                    _temporaryFunctions.CollectionChanged -= OnFunctionsCollectionChanged;
                }

                _temporaryFunctions = value;

                if (_temporaryFunctions is not null)
                {
                    _temporaryFunctions.CollectionChanged += OnFunctionsCollectionChanged;
                }

                OnPropertyChanged(nameof(TemporaryFunctions));
            }
        }

        private void OnFunctionsCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (TemporaryFunctions is null)
            {
                TemporaryFunctionViewModels = null;
                return;
            }

            List<TemporaryFunctionViewModel> temporaryFunctionViewModels = new();

            foreach (var function in TemporaryFunctions)
            {
                temporaryFunctionViewModels.Add(new(TemporaryFunctions, function));
            }

            TemporaryFunctionViewModels = temporaryFunctionViewModels;
        }

        private List<TemporaryFunctionViewModel>? _temporaryFunctionViewModels;
        public List<TemporaryFunctionViewModel>? TemporaryFunctionViewModels
        {
            get { return _temporaryFunctionViewModels; }
            set
            {
                _temporaryFunctionViewModels = value;
                OnPropertyChanged(nameof(TemporaryFunctionViewModels));
            }
        }

        private string? _functionPreview;
        public string? FunctionPreview
        {
            get { return _functionPreview; }
            set
            {
                _functionPreview = value;
                OnPropertyChanged(nameof(FunctionPreview));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand AddMultiFunctionCommand { get; }
        public ICommand SelectionChangedCommand { get; } 

        public CreateMultiFunctionViewModel()
        {
            AddMultiFunctionCommand = new AddMultiFunctionCommand<T>(this);
            SelectionChangedCommand = new RelayCommand(UpdateSelectedFunctions);
        }

        private void UpdateSelectedFunctions(object? e)
        {
            var selected = (IList<object>?)e;

            if (selected is null)
            {
                SelectedFunctionViewModels = null;
                return;
            }
            SelectedFunctionViewModels = selected.Cast<TemporaryFunctionViewModel>().ToList();
        }
    }
}
