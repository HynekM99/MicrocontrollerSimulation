using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Project
{
    public class CurrentProject
    {
        public event Action? CurrentProjectChanged;

        private ProjectInfo _projectInfo = ProjectInfo.GetDefaultProject();
        public ProjectInfo ProjectInfo
        {
            get { return _projectInfo; }
            set
            {
                _projectInfo = value;
                CurrentProjectChanged?.Invoke();
            }
        }
    }
}
