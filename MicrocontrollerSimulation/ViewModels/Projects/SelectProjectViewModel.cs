﻿using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Commands.Projects;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Projects
{
    public class SelectProjectViewModel : ViewModelBase
    {
        private readonly string _projectsDirectory;
        private readonly CurrentProject _currentProject;
        private readonly ILoadingService _loadingService;
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
                Update();
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

        private List<ProjectInfoViewModel> _projects;
        public List<ProjectInfoViewModel> Projects
        {
            get { return _projects; }
            private set
            {
                _projects = value;
                OnPropertyChanged(nameof(Projects));
            }
        }

        public ICommand RefreshProjectsCommand { get; }
        public ICommand OpenProjectsFolderCommand { get; }

        public SelectProjectViewModel(
            string projectsDirectory,
            CurrentProject currentProject,
            ILoadingService loadingService,
            NavigationInitializerService navigationInitializerService)
        {
            _projectsDirectory = projectsDirectory;
            _currentProject = currentProject;
            _loadingService = loadingService;
            _navigationInitializerService = navigationInitializerService;

            RefreshProjectsCommand = new RelayCommand(e => Update());
            OpenProjectsFolderCommand = new OpenProjectsFolderCommand(projectsDirectory);

            _projects = GetAvailableProjects();
            UpdateSearchResults();
        }

        private List<ProjectInfoViewModel> GetAvailableProjects()
        {
            Directory.CreateDirectory(_projectsDirectory);
            string[] projects = Directory.GetFiles(_projectsDirectory, "*.json");

            List<ProjectInfoViewModel> availableProjects = new();

            foreach (var file in projects)
            {
                var lastModified = File.GetLastWriteTime(file);
                ProjectInfo? projectInfo;

                try { projectInfo = _loadingService.Load(file); }
                catch { continue; }

                if (projectInfo is null) continue;

                availableProjects.Add(new(
                    this,
                    projectInfo.Name, 
                    Path.GetFullPath(file), 
                    lastModified));
            }

            return availableProjects;
        }

        private void SelectProject()
        {
            ProjectInfo? project;

            try { project = _loadingService.Load(SelectedProject!.FilePath); }
            catch { project = null; }

            if (project is null)
            {
                MessageBox.Show($"Projekt \"{SelectedProject!.Name}\" se nepodařilo načíst.", "Došlo k chybě");
                return;
            }

            _currentProject.ProjectInfo = project;
            _navigationInitializerService.Navigate();
        }

        public void Update()
        {
            Projects = GetAvailableProjects();
            UpdateSearchResults();
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
