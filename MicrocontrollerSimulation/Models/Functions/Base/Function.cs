using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Functions.Base
{
    public class Function
    {
        public string Name { get; }
        public LogicalExpression Expression { get; }

        public Function(string name, LogicalExpression expression)
        {
            Name = name;
            Expression = expression;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
