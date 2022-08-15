using MicrocontrollerSimulation.JDOs.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs.Functions
{
    public class FunctionJDO
    {
        public string Name { get; set; } = string.Empty;
        public LogicalExpressionJDO LogicalExpression { get; set; } = new();
    }
}
