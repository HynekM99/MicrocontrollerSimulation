using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.SavingServices
{
    public class JsonFileSavingService : ISavingService
    {
        private readonly string _directory;
        private readonly CurrentProject _project;
        private readonly IProjectToJsonService _convertProjectService;

        public JsonFileSavingService(
            string directory,
            CurrentProject project,
            IProjectToJsonService convertProjectService)
        {
            _directory = directory;
            _project = project;
            _convertProjectService = convertProjectService;
        }

        public void Save()
        {
            Directory.CreateDirectory(_directory);
            string fullPath = $@"{_directory}\{_project.ProjectInfo.Name}.json";
            File.WriteAllText(fullPath, _convertProjectService.Convert());
        }
    }
}
