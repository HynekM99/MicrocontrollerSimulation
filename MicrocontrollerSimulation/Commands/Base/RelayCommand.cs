using System;

namespace MicrocontrollerSimulation.Commands.Base
{
    public class RelayCommand : CommandBase
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;

        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public RelayCommand(Action<object?> execute) : this(execute, null)
        {
        }

        public override bool CanExecute(object? parameter)
        {
            return base.CanExecute(parameter) &&
                (_canExecute is null || _canExecute(parameter));
        }

        public override void Execute(object? parameter)
        {
            _execute.Invoke(parameter);
        }
    }
}
