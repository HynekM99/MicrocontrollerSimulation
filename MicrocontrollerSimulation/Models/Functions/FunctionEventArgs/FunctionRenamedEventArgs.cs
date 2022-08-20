using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Functions.FunctionEventArgs
{
    public class FunctionRenamedEventArgs : EventArgs
    {
        public string OldName { get; }
        public string NewName { get; }

        public FunctionRenamedEventArgs(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
}
