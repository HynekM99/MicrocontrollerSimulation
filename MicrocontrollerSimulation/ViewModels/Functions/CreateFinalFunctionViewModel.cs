﻿using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class CreateFinalFunctionViewModel : ViewModelBase
    {
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
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

        private TemporaryFunctionViewModel? _selectedFunctionViewModel;
        public TemporaryFunctionViewModel? SelectedFunctionViewModel
        {
            get { return _selectedFunctionViewModel; }
            set
            {
                _selectedFunctionViewModel = value;
                OnPropertyChanged(nameof(SelectedFunctionViewModel));
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

        public FunctionsCollection Functions { get; }

        public ICommand CreateFinalFunctionCommand { get; }

        public CreateFinalFunctionViewModel(
            FunctionsCollection functions,
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> navigationService)
        {
            Functions = functions;

            CreateFinalFunctionCommand = new CreateCustomFunctionCommand(this, navigationService);
        }
    }
}
