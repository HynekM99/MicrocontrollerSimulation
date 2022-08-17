using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Commands.Projects
{
    public class CreateProjectCommand : CommandBase
    {
        private readonly NewProjectViewModel _newProjectViewModel;
        private readonly CurrentProject _project;
        private readonly NavigationInitializerService _navigationInitializerService;

        public CreateProjectCommand(
            NewProjectViewModel newProjectViewModel, 
            CurrentProject project,
            NavigationInitializerService navigationInitializerService)
        {
            _newProjectViewModel = newProjectViewModel;
            _project = project;
            _navigationInitializerService = navigationInitializerService;

            _newProjectViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_newProjectViewModel.ProjectName))
            {
                OnCanExecuteChanged();
            }
        }

        public override void Execute(object? parameter)
        {
            _project.ProjectInfo = ProjectInfo.GetNewProject(_newProjectViewModel.ProjectName!);
            _navigationInitializerService.Navigate();
        }

        public override bool CanExecute(object? parameter)
        {
            string? name = _newProjectViewModel.ProjectName;

            if (string.IsNullOrWhiteSpace(name))
            {
                _newProjectViewModel.ErrorMessage = null;
                return false;
            }

            if (!name.All(c => char.IsLetterOrDigit(c) || c == '_'))
            {
                _newProjectViewModel.ErrorMessage = "Název musí obsahovat pouze písmena, čísla a podtržítka.";
                return false;
            }

            if (_newProjectViewModel.GetExistingProjects().Any(p => p == name))
            {
                _newProjectViewModel.ErrorMessage = "Projekt s tímto názvem již existuje.";
                return false;
            }

            _newProjectViewModel.ErrorMessage = null;
            return base.CanExecute(parameter);
        }
    }
}
