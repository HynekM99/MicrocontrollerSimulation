using MicrocontrollerSimulation.Services.ProjectConversionServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.LoadingServices
{
    public class FileLoadingService : ILoadingService
    {
        private readonly string _directory;

        public FileLoadingService(string directory)
        {
            _directory = directory;
        }

        public string Load(string fileName)
        {
            string filePath = $@"{_directory}\{fileName}";
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Project save file could not be found.", filePath);
            }

            return File.ReadAllText(filePath);
        }
    }
}
