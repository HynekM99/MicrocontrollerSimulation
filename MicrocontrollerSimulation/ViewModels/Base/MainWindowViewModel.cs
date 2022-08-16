using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.LoadingServices;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using MicrocontrollerSimulation.Services.SavingServices;
using MicrocontrollerSimulation.Stores;
using System.Windows.Input;

namespace MicrocontrollerSimulation.ViewModels.Base
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const string TITLE = "Microcontroller Simulation";

        private readonly CurrentProject _currentProject;
        private readonly ISavingService _savingService;
        private readonly ILoadingService _loadingService;
        private readonly IJsonToProjectService _unconvertProjectService;
        private readonly NavigationStore<MainWindowViewModel> _navigationStore;

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
            ISavingService savingService, 
            ILoadingService loadingService,
            IJsonToProjectService unconvertProjectService,
            NavigationStore<MainWindowViewModel> navigationStore,
            NavigationInitializerService navigationInitializerService)
        {
            _currentProject = currentProject;
            _savingService = savingService;
            _loadingService = loadingService;
            _unconvertProjectService = unconvertProjectService;
            _navigationStore = navigationStore;

            NewProjectCommand = new RelayCommand(e =>
            {
                currentProject.ProjectInfo = ProjectInfo.GetDefaultProject();
                navigationInitializerService.Navigate();
            });

            OpenProjectCommand = new RelayCommand(e =>
            {
                string json = _loadingService.Load("default_project.json");
                currentProject.ProjectInfo = _unconvertProjectService.Unconvert(json);
                navigationInitializerService.Navigate();
            });

            SaveProjectCommand = new RelayCommand(e =>
            {
                _savingService.Save();
            });

            currentProject.CurrentProjectChanged += OnCurrentProjectChanged;
            _navigationStore.CurrentViewModelChanged += () => OnPropertyChanged(nameof(CurrentViewModel));
        }

        private void OnCurrentProjectChanged()
        {
            Title = $"{_currentProject.ProjectInfo.Name} - {TITLE}"; 
        }
    }
}
