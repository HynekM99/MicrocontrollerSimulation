﻿using MicrocontrollerSimulation.Services.SavingServices;
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

        public bool HasUnsavedChanges { get; private set; } = false;

        private ProjectInfo _projectInfo = ProjectInfo.GetNewProject();
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
            _projectInfo.ProjectEdited += OnProjectEdited;
        }

        public void Save()
        {
            _savingService.Save(this);
            HasUnsavedChanges = false;
            ProjectEdited?.Invoke();
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
