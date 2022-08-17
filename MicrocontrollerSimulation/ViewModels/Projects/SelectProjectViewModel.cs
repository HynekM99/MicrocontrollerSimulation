using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.ViewModels.Projects
{
    public class SelectProjectViewModel : ViewModelBase
    {
        private readonly string _projectsDirectory;
        private readonly CurrentProject _currentProject;
        private readonly ILoadingService _loadingService;
        private readonly IJsonToProjectService _jsonToProjectService;
        private readonly NavigationInitializerService _navigationInitializerService;

        private ProjectInfoViewModel? _selectedProject;
        public ProjectInfoViewModel? SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                SelectProject();
            }
        }

        private string? _searchedProjectName;
        public string? SearchedProjectName
        {
            get { return _searchedProjectName; }
            set
            {
                _searchedProjectName = value;
                OnPropertyChanged(nameof(SearchedProjectName));
                UpdateSearchResults();
            }
        }

        public List<ProjectInfoViewModel> _searchedProjects = new();
        public List<ProjectInfoViewModel> SearchedProjects
        {
            get { return _searchedProjects; }
            set
            {
                _searchedProjects = value;
                OnPropertyChanged(nameof(SearchedProjects));
            }
        }

        public List<ProjectInfoViewModel> Projects { get; }

        public SelectProjectViewModel(
            string projectsDirectory,
            CurrentProject currentProject,
            ILoadingService loadingService,
            IJsonToProjectService jsonToProjectService,
            NavigationInitializerService navigationInitializerService)
        {
            _projectsDirectory = projectsDirectory;
            _currentProject = currentProject;
            _loadingService = loadingService;
            _jsonToProjectService = jsonToProjectService;
            _navigationInitializerService = navigationInitializerService;

            Projects = GetAvailableProjects();
            UpdateSearchResults();
        }

        private List<ProjectInfoViewModel> GetAvailableProjects()
        {
            string[] projects = Directory.GetFiles(_projectsDirectory, "*.json");
            
            List<ProjectInfoViewModel> availableProjects = new();

            foreach (var file in projects)
            {
                var lastModified = File.GetLastWriteTime(file);
                string fileName = Path.GetFileNameWithoutExtension(file);
                
                string json = _loadingService.Load($"{fileName}.json");
                var projectInfo = _jsonToProjectService.Unconvert(json);

                availableProjects.Add(new ProjectInfoViewModel(projectInfo.Name, Path.GetFullPath(file), lastModified));
            }

            return availableProjects;
        }

        private void SelectProject()
        {
            string json = _loadingService.Load($"{SelectedProject!.Name}.json");
            _currentProject.ProjectInfo = _jsonToProjectService.Unconvert(json);
            _navigationInitializerService.Navigate();
        }

        private void UpdateSearchResults()
        {
            var projects = !string.IsNullOrWhiteSpace(SearchedProjectName) ?
                Projects.Where(e => e.Name.Contains(SearchedProjectName)) :
                Projects;

            SearchedProjects = projects
                .OrderByDescending(e => e.LastModified)
                .ToList();
        }
    }
}
