using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCUSimulation.Models.LogicalExpressions.Custom
{
    public class CustomExpression : LogicalExpression
    {
        private readonly LogicalExpression _expression;

        public override ICollection<Input> Inputs => _expression.Inputs;
        public override bool Result => _expression.Result;
        public override string AsString => _expression.AsString;

        public CustomExpression(LogicalExpression expression)
        {
            _expression = expression;
        }
    }
}
