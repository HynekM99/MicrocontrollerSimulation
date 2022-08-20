using MicrocontrollerSimulation.Commands.FunctionEditing;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Functions.Editing
{
    public class FunctionEditViewModel : ViewModelBase
    {
        private readonly FunctionsCollection _functions;

        private Function? _function;
        public Function? Function
        {
            get { return _function; }
            set
            {
                _function = value;
                OnPropertyChanged(nameof(Function));

                NewName = _function!.Name;
            }
        }

        private string? _newName;
        public string? NewName
        {
            get { return _newName; }
            set
            {
                _newName = value;
                OnPropertyChanged(nameof(NewName));
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

        public ICommand ConfirmEditCommand { get; }

        public FunctionEditViewModel(FunctionsCollection functions)
        {
            _functions = functions;

            ConfirmEditCommand = new EditFunctionCommand(this, functions);
        }
    }
}
