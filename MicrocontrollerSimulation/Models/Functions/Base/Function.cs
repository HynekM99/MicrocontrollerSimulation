using MicrocontrollerSimulation.Models.Functions.FunctionEventArgs;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;

namespace MicrocontrollerSimulation.Models.Functions.Base
{
    public class Function
    {
        public event EventHandler<FunctionRenamedEventArgs>? FunctionRenamed;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException(value);
                }

                if (_name != value)
                {
                    string oldValue = _name;
                    _name = value;

                    FunctionRenamed?.Invoke(this, new(oldValue, _name));
                }
            }
        }
        public LogicalExpression Expression { get; }

        public Function(string name, LogicalExpression expression)
        {
            _name = name;
            Expression = expression;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
