using MicrocontrollerSimulation.Models.LogicalExpressions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices
{
    public interface IExpressionParser
    {
        LogicalExpression Parse(string expression);
    }
}
