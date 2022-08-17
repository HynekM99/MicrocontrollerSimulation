using MicrocontrollerSimulation.Models.Project;
using MicrocontrollerSimulation.Services.ProjectConversionServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.LoadingServices
{
    public class JsonLoadingService : ILoadingService
    {
        private readonly IJsonToProjectService _jsonToProjectService;

        public JsonLoadingService(IJsonToProjectService jsonToProjectService)
        {
            _jsonToProjectService = jsonToProjectService;
        }

        public ProjectInfo? Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Project save file could not be found.", filePath);
            }

            return _jsonToProjectService.Unconvert(File.ReadAllText(filePath));
        }
    }
}
