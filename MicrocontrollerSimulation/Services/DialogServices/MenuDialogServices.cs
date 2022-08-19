using MicrocontrollerSimulation.Views.Windows;
using MicrocontrollerSimulation.Views.Windows.Simulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.DialogServices
{
    public class MenuDialogServices
    {
        public DialogService<SelectProjectWindow> SelectProjectDialogService;
        public DialogService<NewProjectWindow> NewProjectDialogService;
        public DialogService<SimulationWindow> SimulationDialogService;
        public DialogService<AboutAppWindow> AboutAppDialogService;

        public MenuDialogServices(
            DialogService<SelectProjectWindow> selectProjectDialogService, 
            DialogService<NewProjectWindow> newProjectDialogService, 
            DialogService<SimulationWindow> simulationDialogService,
            DialogService<AboutAppWindow> aboutAppDialogService)
        {
            SelectProjectDialogService = selectProjectDialogService;
            NewProjectDialogService = newProjectDialogService;
            SimulationDialogService = simulationDialogService;
            AboutAppDialogService = aboutAppDialogService;
        }
    }
}
