using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.LogicalExpressions.Base
{
    public abstract class LogicalExpression
    {
        public abstract ICollection<Input> Inputs { get; }
        public abstract bool Result { get; }
        public abstract string AsString { get; }

        protected static void CheckForInputDuplicity(LogicalExpression expression1, LogicalExpression expression2)
        {
            List<Input> inputs1 = expression1.Inputs.ToList();
            List<Input> inputs2 = expression2.Inputs.ToList();

            foreach (var input in inputs1)
            {
                int index = inputs2.IndexOf(input);
                if (index > -1 && input != inputs2[index])
                {
                    throw new ArgumentException($"Expression cannot contain two different inputs with the same name ({input})");
                }
            }
        }

        public override bool Equals(object? obj)
        {
            return obj is LogicalExpression expression &&
                   ToString() == expression.ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ToString());
        }

        public override string ToString()
        {
            return AsString;
        }
    }
}
