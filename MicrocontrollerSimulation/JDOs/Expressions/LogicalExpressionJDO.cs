using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs.Expressions
{
    public class LogicalExpressionJDO
    {
        public HashSet<InputJDO> Inputs { get; set; } = new();
        public List<LogicalExpressionJDO> LogicalExpressions { get; set; } = new();
    }
}
