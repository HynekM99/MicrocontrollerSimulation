using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.SavingServices
{
    public class FileSavingService : ISavingService
    {
        private readonly string _directory;
        private readonly string _fullPath;
        private readonly IConvertProjectService _convertProjectService;

        public FileSavingService(
            string fileName,
            string directory,
            IConvertProjectService convertProjectService)
        {
            _directory = directory;
            _fullPath = $@"{directory}\{fileName}";
            _convertProjectService = convertProjectService;
        }

        public void Save()
        {
            Directory.CreateDirectory(_directory);
            File.WriteAllText(_fullPath, _convertProjectService.Convert());
        }
    }
}
