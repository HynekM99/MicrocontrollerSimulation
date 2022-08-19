using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.Services.SavingServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.Views;
using MicrocontrollerSimulation.Views.Windows;
using MicrocontrollerSimulation.Views.Windows.Simulation;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Base
{
    public class MainWindowViewModel : ViewModelBase
    {
        public event Action? CloseWindow;

        private readonly CurrentProject _currentProject;
        private readonly NavigationStore<MainWindowViewModel> _navigationStore;
        private readonly DialogService<SelectProjectWindow> _selectProjectDialogService;
        private readonly DialogService<NewProjectWindow> _newProjectDialogService;
        private readonly DialogService<SimulationWindow> _simulationDialogService;

        private string _projectName = "";
        public string ProjectName
        {
            get { return _projectName; }
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NewProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand SaveProjectCommand { get; }
        public ICommand StartSimulationCommand { get; }
        public ICommand OpenAboutAppCommand { get; }
        public ICommand CloseWindowCommand { get; }

        public MainWindowViewModel(
            CurrentProject currentProject,
            NavigationStore<MainWindowViewModel> navigationStore,
            MenuDialogServices menuDialogServices)
        {
            _currentProject = currentProject;
            _navigationStore = navigationStore;
            _selectProjectDialogService = menuDialogServices.SelectProjectDialogService;
            _newProjectDialogService = menuDialogServices.NewProjectDialogService;
            _simulationDialogService = menuDialogServices.SimulationDialogService;

            NewProjectCommand = new RelayCommand(e => TryChangeProject(() => _newProjectDialogService.ShowDialog()));
            OpenProjectCommand = new RelayCommand(e => TryChangeProject(() => _selectProjectDialogService.ShowDialog()));
            SaveProjectCommand = new RelayCommand(e => TrySaveProject());
            StartSimulationCommand = new RelayCommand(e => _simulationDialogService.ShowDialog());
            OpenAboutAppCommand = new RelayCommand(e => menuDialogServices.AboutAppDialogService.Show());
            CloseWindowCommand = new RelayCommand(e => TryChangeProject(() => CloseWindow?.Invoke()));

            SetTitle();

            _currentProject.ProjectEdited += OnProjectEdited;
            _currentProject.CurrentProjectChanged += OnCurrentProjectChanged;
            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
        }

        private void SetTitle()
        {
            ProjectName = $"{(_currentProject.HasUnsavedChanges ? "*" : "")}{_currentProject.ProjectInfo.Name}";
        }

        private void OnProjectEdited()
        {
            SetTitle();
        }

        private void OnCurrentProjectChanged()
        {
            SetTitle();
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void TryChangeProject(Action action)
        {
            if (!_currentProject.HasUnsavedChanges)
            {
                action.Invoke();
                return;
            }

            var result = MessageBox.Show("Chcete uložit změny v projektu?", "Neuložené změny", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                TrySaveProject();
            }
            else if (result != MessageBoxResult.No)
            {
                return;
            }
            action.Invoke();
        }

        private void TrySaveProject()
        {
            try { _currentProject.Save(); }
            catch (IOException ex) { MessageBox.Show(ex.Message); }
        }

        public override void Dispose()
        {
            _currentProject.ProjectEdited -= OnProjectEdited;
            _currentProject.CurrentProjectChanged -= OnCurrentProjectChanged;
            _navigationStore.CurrentViewModelChanged -= OnCurrentViewModelChanged;
            base.Dispose();
        }
    }
}
