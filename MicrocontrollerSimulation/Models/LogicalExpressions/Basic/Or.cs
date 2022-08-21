using MicrocontrollerSimulation.ExtensionMethods;
using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Basic
{
    public class Or : LogicalExpression
    {
        private static readonly char EXPRESSION_SIGN = '+';

        public override ReadOnlyCollection<LogicalExpression> Subexpressions { get; protected set; }
        public override HashSet<Input> Inputs { get; } = new HashSet<Input>();

        public override bool Result
        {
            get
            {
                bool result = false;
                Subexpressions.ForEach(e => result |= e.Result);
                return result;
            }
        }
        public override string AsString
        {
            get
            {
                string s = "";
                Subexpressions.ForEach(e => s += $"{e.AsString} {EXPRESSION_SIGN} ");
                s = s.Trim().Trim(EXPRESSION_SIGN).Trim();
                return $"({s})";
            }
        }

        public Or(List<LogicalExpression> expressions)
        {
            if (expressions.Count < 2)
            {
                throw new ArgumentException("The Or expression must be composed of at least 2 other expressions.");
            }

            expressions.ForEach(e =>
            {
                e.Inputs.ForEach(i => Inputs.Add(i));
            });

            Subexpressions = new(expressions);

            if (ContainsDuplicateInputs())
            {
                throw new ArgumentException($"Expression cannot contain two different inputs with the same name.");
            }
        }

        public Or(params LogicalExpression[] expressions) : this(new List<LogicalExpression>(expressions))
        {
        }
    }
}
