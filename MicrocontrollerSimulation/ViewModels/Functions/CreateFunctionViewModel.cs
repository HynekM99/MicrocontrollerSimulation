using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.FunctionCreation;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions
{
    public class CreateFunctionViewModel : ViewModelBase
    {
        private string? _newInputName;
        public string? NewInputName
        {
            get { return _newInputName; }
            set
            {
                _newInputName = value;
                OnPropertyChanged(nameof(NewInputName));
            }
        }

        private string? _inputNameErrorMessage;
        public string? InputNameErrorMessage
        {
            get { return _inputNameErrorMessage; }
            set
            {
                _inputNameErrorMessage = value;
                OnPropertyChanged(nameof(InputNameErrorMessage));
            }
        }

        public bool CreatorVisible => Functions.Any();

        public FunctionsCollection Functions { get; } = new();

        public CreateNotFunctionViewModel CreateNotFunctionViewModel { get; }
        public CreateMultiFunctionViewModel<And> CreateAndFunctionViewModel { get; }
        public CreateMultiFunctionViewModel<Or> CreateOrFunctionViewModel { get; }
        public CreateMultiFunctionViewModel<Xor> CreateXorFunctionViewModel { get; }
        public CreateFinalFunctionViewModel SelectFinalFunctionViewModel { get; }

        public ICommand CancelCommand { get; }
        public ICommand AddInputCommand { get; }

        public CreateFunctionViewModel(
            CreateNotFunctionViewModel createNotFunctionViewModel,
            CreateMultiFunctionViewModel<And> createAndFunctionViewModel,
            CreateMultiFunctionViewModel<Or> createOrFunctionViewModel,
            CreateMultiFunctionViewModel<Xor> createXorFunctionViewModel,
            CreateFinalFunctionViewModel selectFinalFunctionViewModel,
            NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel> functionsOverviewNavigationService)
        {
            CreateNotFunctionViewModel = createNotFunctionViewModel;
            CreateAndFunctionViewModel = createAndFunctionViewModel;
            CreateOrFunctionViewModel = createOrFunctionViewModel;
            CreateXorFunctionViewModel = createXorFunctionViewModel;
            SelectFinalFunctionViewModel = selectFinalFunctionViewModel;

            CreateNotFunctionViewModel.Functions = Functions;
            CreateAndFunctionViewModel.Functions = Functions;
            CreateOrFunctionViewModel.Functions = Functions;
            CreateXorFunctionViewModel.Functions = Functions;
            SelectFinalFunctionViewModel.TemporaryFunctions = Functions;

            CancelCommand = new CancelFunctionCreationCommand(this, functionsOverviewNavigationService);
            AddInputCommand = new AddInputCommand(this);
            
            Functions.CollectionChanged += (s, e) => OnPropertyChanged(nameof(CreatorVisible));
        }
    }
}
