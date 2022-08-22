using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions
{
    public class MissingInputException : Exception
    {
        public MissingInputException() : base("Missing an input name.")
        {
        }
    }
}
