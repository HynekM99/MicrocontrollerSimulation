using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.DialogServices;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.Services.SavingServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.Views;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Base
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string TITLE = "Microcontroller Simulation";

        private readonly CurrentProject _currentProject;
        private readonly NavigationStore<MainWindowViewModel> _navigationStore;
        private readonly DialogService<SelectProjectWindow> _selectProjectDialogService;

        private string _title = TITLE;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

        public ICommand NewProjectCommand { get; }
        public ICommand OpenProjectCommand { get; }
        public ICommand SaveProjectCommand { get; }

        public MainWindowViewModel(
            CurrentProject currentProject,
            NavigationStore<MainWindowViewModel> navigationStore,
            NavigationInitializerService navigationInitializerService,
            DialogService<SelectProjectWindow> selectProjectDialogService)
        {
            _currentProject = currentProject;
            _navigationStore = navigationStore;
            _selectProjectDialogService = selectProjectDialogService;

            NewProjectCommand = new RelayCommand(e =>
            {
                currentProject.ProjectInfo = ProjectInfo.GetDefaultProject();
                navigationInitializerService.Navigate();
            });

            OpenProjectCommand = new RelayCommand(e =>
            {
                _selectProjectDialogService.ShowDialog();
            });

            SaveProjectCommand = new RelayCommand(e =>
            {
                _currentProject.Save();
            });

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
            Title = $"{_currentProject.ProjectInfo.Name} - {TITLE}";
        }
    }
}
