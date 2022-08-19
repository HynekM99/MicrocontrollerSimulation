using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.ViewModels.Projects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MicrocontrollerSimulation.Commands.Projects
{
    public class DeleteProjectCommand : CommandBase
    {
        private readonly SelectProjectViewModel _selectProjectViewModel;
        private readonly ProjectInfoViewModel _projectInfoViewModel;

        public DeleteProjectCommand(ProjectInfoViewModel projectInfoViewModel, SelectProjectViewModel selectProjectViewModel)
        {
            _projectInfoViewModel = projectInfoViewModel;
            _selectProjectViewModel = selectProjectViewModel;
        }

        public override void Execute(object? parameter)
        {
            var name = _projectInfoViewModel.Name;
            var path = _projectInfoViewModel.FilePath;

            if (MessageBox.Show($"Opravdu chcete trvale smazat projekt \"{name}\"", "Potvrzení", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                File.Delete(path);
                _selectProjectViewModel.Update();
            }
        }
    }
}
