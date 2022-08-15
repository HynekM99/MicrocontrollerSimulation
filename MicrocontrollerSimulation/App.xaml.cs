using MicrocontrollerSimulation.Models.LogicalExpressions.Custom;
using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Services.NavigationServices;
using MicrocontrollerSimulation.Stores;
using MicrocontrollerSimulation.ViewModels;
using MicrocontrollerSimulation.ViewModels.Base;
using MicrocontrollerSimulation.ViewModels.Functions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using MicrocontrollerSimulation.ViewModels.Microcontrollers;
using MicrocontrollerSimulation.Models.Microcontrollers;
using MicrocontrollerSimulation.Models.InputDevices.Factories;
using MicrocontrollerSimulation.Models.Functions.Provider;
using MicrocontrollerSimulation.Models.Microcontrollers.Pins;
using MicrocontrollerSimulation.Services.SavingServices.ProjectConversionServices;
using MicrocontrollerSimulation.Services.SavingServices;

namespace MicrocontrollerSimulation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Only for testing purposes.
        private const string PROJECT_NAME = "test_project";
        private const string PROJECTS_DIRECTORY = @".\projects\";

        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services.AddSingleton<FunctionsCollection>();
                services.AddSingleton<Microcontroller>();
                services.AddSingleton<IDeviceFactory, BasicDeviceFactory>();
                services.AddSingleton<IFunctionsProvider, FunctionsProvider>();
                services.AddSingleton<IConvertProjectService>(s =>
                {
                    return new ProjectToJsonService(PROJECT_NAME, s.GetRequiredService<FunctionsCollection>());
                });
                services.AddSingleton<ISavingService>(s =>
                {
                    return new FileSavingService($"{PROJECT_NAME}.json", PROJECTS_DIRECTORY, s.GetRequiredService<IConvertProjectService>());
                });

                services.AddSingleton<NavigationStore<MainWindowViewModel>>();
                services.AddSingleton<NavigationStore<FunctionsSetupViewModel>>();
                services.AddSingleton<NavigationStore<MicrocontrollerSetupViewModel>>();
                services.AddSingleton<NavigationStore<PinsOverviewViewModel>>();

                services.AddSingleton<NavigationService<FunctionsSetupViewModel, FunctionsOverviewViewModel>>();
                services.AddSingleton<NavigationService<FunctionsSetupViewModel, CreateFunctionViewModel>>();

                services.AddSingleton<NavigationService<PinsOverviewViewModel, SelectedPinConfigurationViewModel>>();

                services.AddSingleton<Func<FunctionsOverviewViewModel>>(s => () => s.GetRequiredService<FunctionsOverviewViewModel>());
                services.AddSingleton<Func<CreateFunctionViewModel>>(s => () => s.GetRequiredService<CreateFunctionViewModel>());
                services.AddSingleton<Func<SelectedPinConfigurationViewModel>>(s => () => s.GetRequiredService<SelectedPinConfigurationViewModel>());

                services.AddSingleton<MainViewModel>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton<FunctionsSetupViewModel>();
                services.AddTransient<FunctionsOverviewViewModel>();

                services.AddTransient<CreateFunctionViewModel>();
                services.AddTransient<CreateNotFunctionViewModel>();
                services.AddSingleton<CreateMultiFunctionViewModel<And>>();
                services.AddTransient<CreateMultiFunctionViewModel<Or>>();
                services.AddTransient<CreateMultiFunctionViewModel<Xor>>();
                services.AddTransient<CreateFinalFunctionViewModel>();

                services.AddSingleton<MicrocontrollerSetupViewModel>();
                services.AddSingleton<PinsOverviewViewModel>();

                services.AddTransient(s =>
                {
                    return new SelectedPinInputModeConfigViewModel(
                        s.GetRequiredService<PinsOverviewViewModel>().SelectedPin,
                        s.GetRequiredService<IDeviceFactory>());
                });

                services.AddTransient(s =>
                {
                    return new SelectedPinOutputModeConfigViewModel(
                        s.GetRequiredService<PinsOverviewViewModel>().SelectedPin,
                        s.GetRequiredService<IFunctionsProvider>());
                });

                services.AddTransient(s =>
                {
                    return new SelectedPinConfigurationViewModel(
                        s.GetRequiredService<PinsOverviewViewModel>().SelectedPin,
                        s.GetRequiredService<SelectedPinInputModeConfigViewModel>(),
                        s.GetRequiredService<SelectedPinOutputModeConfigViewModel>());
                });

                services.AddSingleton(s => new MainWindow() { DataContext = s.GetRequiredService<MainWindowViewModel>() });
            }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            SetupNavigationState();

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);

            AddBasicFunctions();
            AddCustomFunction();

            Exit += OnAppExit;
        }

        private void SetupNavigationState()
        {
            var mainNavigationStore = _host.Services.GetRequiredService<NavigationStore<MainWindowViewModel>>();
            mainNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<MainViewModel>();

            var functionsNavigationStore = _host.Services.GetRequiredService<NavigationStore<FunctionsSetupViewModel>>();
            functionsNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<FunctionsOverviewViewModel>();

            var microcontrollerNavigationStore = _host.Services.GetRequiredService<NavigationStore<MicrocontrollerSetupViewModel>>();
            microcontrollerNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<PinsOverviewViewModel>();

            var pinsOverviewNavigationStore = _host.Services.GetRequiredService<NavigationStore<PinsOverviewViewModel>>();
            pinsOverviewNavigationStore.CurrentViewModel = _host.Services.GetRequiredService<SelectedPinConfigurationViewModel>();
        }

        private void AddBasicFunctions()
        {
            Not not = new(new Input("IN"));
            And and = new(new Input("A"), new Input("B"));
            Or or = new(new Input("A"), new Input("B"));
            Xor xor = new(new Input("A"), new Input("B"));

            var notFunction = new Function("Not", not);
            var andFunction = new Function("And", and);
            var orFunction = new Function("Or", or);
            var xorFunction = new Function("Xor", xor);

            var functions = _host.Services.GetRequiredService<FunctionsCollection>();
            functions.Add(notFunction);
            functions.Add(andFunction);
            functions.Add(orFunction);
            functions.Add(xorFunction);
        }

        private void AddCustomFunction()
        {
            Input a = new("A");
            Input b = new("B");
            Input c = new("C");

            And and1 = new(a, b);
            And and2 = new(new Not(a), c);
            And and3 = new(a, b);

            Or or1 = new(and1, c);
            Or or2 = new(and2, a);

            Xor xor = new(or1, and3);

            And and = new(xor, or2);

            var customExpression = new CustomExpression(and);

            var customFunction = new Function("Test_function", customExpression);

            _host.Services.GetRequiredService<FunctionsCollection>().Add(customFunction);
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            var savingService = _host.Services.GetRequiredService<ISavingService>();
            savingService.Save();
        }
    }
}
