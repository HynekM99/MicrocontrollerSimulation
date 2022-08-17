using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Projects
{
    public class ProjectInfoViewModel : ViewModelBase
    {
        public string Name { get; }
        public string FilePath { get; }
        public DateTime LastModified { get; }

        public ProjectInfoViewModel(string name, string filePath, DateTime lastModified)
        {
            Name = name;
            FilePath = filePath;
            LastModified = lastModified;
        }
    }
}
