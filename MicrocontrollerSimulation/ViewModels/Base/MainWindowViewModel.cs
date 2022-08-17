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
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Base
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly CurrentProject _currentProject;
        private readonly NavigationStore<MainWindowViewModel> _navigationStore;
        private readonly DialogService<SelectProjectWindow> _selectProjectDialogService;
        private readonly DialogService<NewProjectWindow> _newProjectDialogService;

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
        public ICommand OpenAboutAppCommand { get; }

        public MainWindowViewModel(
            CurrentProject currentProject,
            NavigationStore<MainWindowViewModel> navigationStore,
            DialogService<NewProjectWindow> newProjectDialogService,
            DialogService<SelectProjectWindow> selectProjectDialogService,
            DialogService<AboutAppWindow> aboutAppDialogService)
        {
            _currentProject = currentProject;
            _navigationStore = navigationStore;
            _selectProjectDialogService = selectProjectDialogService;
            _newProjectDialogService = newProjectDialogService;

            NewProjectCommand = new RelayCommand(e =>
            {
                _newProjectDialogService.ShowDialog();
            });

            OpenProjectCommand = new RelayCommand(e =>
            {
                _selectProjectDialogService.ShowDialog();
            });

            SaveProjectCommand = new RelayCommand(e =>
            {
                try
                {
                    _currentProject.Save();
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

            OpenAboutAppCommand = new RelayCommand(e => aboutAppDialogService.Show());
            SetTitle();

            currentProject.CurrentProjectChanged += OnCurrentProjectChanged;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnCurrentProjectChanged()
        {
            SetTitle();
        }

        private void SetTitle()
        {
            ProjectName = _currentProject.ProjectInfo.Name;
        }
    }
}
