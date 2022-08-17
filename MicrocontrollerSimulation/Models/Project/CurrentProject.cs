using MicrocontrollerSimulation.Services.SavingServices;
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

        private readonly ISavingService _savingService;

        private ProjectInfo _projectInfo = ProjectInfo.GetNewProject("default_project");
        public ProjectInfo ProjectInfo
        {
            get { return _projectInfo; }
            set
            {
                _projectInfo = value;
                CurrentProjectChanged?.Invoke();
            }
        }

        public CurrentProject(ISavingService savingService)
        {
            _savingService = savingService;
        }

        public void Save()
        {
            _savingService.Save(this);
        }
    }
}
