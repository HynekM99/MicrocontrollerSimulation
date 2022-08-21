using MicrocontrollerSimulation.Commands.FunctionEditing;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace MicrocontrollerSimulation.ViewModels.Functions.Editing
{
    public class InputEditViewModel : ViewModelBase
    {
        public Input Input { get; }

        private string? _newName;
        public string? NewName
        {
            get { return _newName; }
            set
            {
                if (_newName != value)
                {
                    _newName = value;
                    OnPropertyChanged(nameof(NewName));
                }
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if (value != _errorMessage)
                {
                    _errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }

        public ICommand ConfirmEditCommand { get; }

        public InputEditViewModel(
            Input input, 
            HashSet<Input> inputs)
        {
            Input = input;

            NewName = input.Name;

            ConfirmEditCommand = new EditInputCommand(this, inputs);
        }
    }
}
