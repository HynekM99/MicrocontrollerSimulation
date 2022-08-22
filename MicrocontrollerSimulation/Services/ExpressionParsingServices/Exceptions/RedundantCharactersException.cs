using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ExpressionParsingServices.Exceptions
{
    public class RedundantCharactersException : Exception
    {
        public RedundantCharactersException() : base("The expression contains redundant characters.")
        {
        }
    }
}
