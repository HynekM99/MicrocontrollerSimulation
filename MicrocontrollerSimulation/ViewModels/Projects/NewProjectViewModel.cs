using MicrocontrollerSimulation.Commands.Projects;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Projects
{
    public class NewProjectViewModel : ViewModelBase
    {
        private readonly string _projectsDirectory;

        private string? _projectName;
        public string? ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string? _errorMessage;
        public string? ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand CreateProjectCommand { get; }

        public NewProjectViewModel(
            string projectsDirectory,
            CurrentProject currentProject,
            NavigationInitializerService navigationInitializerService)
        {
            _projectsDirectory = projectsDirectory;

            CreateProjectCommand = new CreateProjectCommand(this, currentProject, navigationInitializerService);
        }

        public List<string> GetExistingProjects()
        {
            Directory.CreateDirectory(_projectsDirectory);
            string[] projects = Directory.GetFiles(_projectsDirectory, "*.json");

            List<string> availableProjects = new();

            foreach (var file in projects)
            {
                availableProjects.Add(Path.GetFileNameWithoutExtension(file));
            }

            return availableProjects;
        }
    }
}
