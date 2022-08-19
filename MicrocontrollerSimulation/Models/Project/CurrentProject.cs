using MicrocontrollerSimulation.Services.SavingServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Project
{
    public class CurrentProject : IDisposable
    {
        public event Action? CurrentProjectChanged;
        public event Action? ProjectEdited;

        private readonly ISavingService _savingService;

        public bool HasUnsavedChanges { get; private set; } = true;

        private ProjectInfo _projectInfo = ProjectInfo.GetNewProject("new_project");
        public ProjectInfo ProjectInfo
        {
            get { return _projectInfo; }
            set
            {
                if (_projectInfo is not null)
                {
                    _projectInfo.ProjectEdited -= OnProjectEdited;
                }

                _projectInfo = value;

                if (_projectInfo is not null)
                {
                    _projectInfo.ProjectEdited += OnProjectEdited;
                    HasUnsavedChanges = false;
                }

                CurrentProjectChanged?.Invoke();
            }
        }

        private void OnProjectEdited()
        {
            HasUnsavedChanges = true;
            ProjectEdited?.Invoke();
        }

        public CurrentProject(ISavingService savingService)
        {
            _savingService = savingService;
        }

        public void Save()
        {
            _savingService.Save(this);
        }

        public void Dispose()
        {
            if (_projectInfo is not null)
            {
                _projectInfo.ProjectEdited -= OnProjectEdited;
            }
            GC.SuppressFinalize(this);
        }
    }
}
