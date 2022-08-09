using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class And : LogicalExpression
    {
        private readonly LogicalExpression _expression1;
        private readonly LogicalExpression _expression2;

        public override bool Result => _expression1.Result && _expression2.Result;
        public override string AsString => $"({_expression1.AsString} * {_expression2.AsString})";

        private ICollection<Input> _inputs = new HashSet<Input>();
        public override ICollection<Input> Inputs => _inputs;

        public And(LogicalExpression expression1, LogicalExpression expression2)
        {
            CheckForInputDuplicity(expression1, expression2);
            _expression1 = expression1;
            _expression2 = expression2;

            foreach (var input in _expression1.Inputs)
            {
                _inputs.Add(input);
            }
            foreach (var input in _expression2.Inputs)
            {
                _inputs.Add(input);
            }
        }
    }
}
