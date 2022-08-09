using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System.Collections.Generic;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Or : LogicalExpression
    {
        private readonly LogicalExpression _expression1;
        private readonly LogicalExpression _expression2;

        public override ICollection<Input> Inputs
        {
            get
            {
                HashSet<Input> inputs = new();
                foreach (var input in _expression1.Inputs)
                {
                    inputs.Add(input);
                }
                foreach (var input in _expression2.Inputs)
                {
                    inputs.Add(input);
                }
                return inputs;
            }
        }
        public override bool Result => _expression1.Result || _expression2.Result;
        public override string AsString => $"({_expression1.AsString} + {_expression2.AsString})";


        public Or(LogicalExpression expression1, LogicalExpression expression2)
        {
            CheckForInputDuplicity(expression1, expression2);
            _expression1 = expression1;
            _expression2 = expression2;
        }
    }
}
