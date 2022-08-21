using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Functions.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.FunctionEditing
{
    public class EditInputCommand : CommandBase
    {
        private readonly InputEditViewModel _inputEditViewModel;
        private readonly Input _input;
        private readonly HashSet<Input> _inputs;

        public EditInputCommand(InputEditViewModel inputEditViewModel, HashSet<Input> inputs)
        {
            _inputEditViewModel = inputEditViewModel;
            _input = inputEditViewModel.Input;
            _inputs = inputs;

            _inputEditViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        public override bool CanExecute(object? parameter)
        {
            var name = _inputEditViewModel.NewName;

            if (string.IsNullOrWhiteSpace(name))
            {
                _inputEditViewModel.ErrorMessage = "Název nesmí být prázdný.";
                return false;
            }

            if (!name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                _inputEditViewModel.ErrorMessage = "Název musí obsahovat pouze písmena, čísla a podtržítka.";
                return false;
            }

            _inputEditViewModel.ErrorMessage = null;
            return base.CanExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _input.Name = _inputEditViewModel.NewName!;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == _inputEditViewModel.NewName)
            {
                OnCanExecuteChanged();
            }
        }
    }
}
