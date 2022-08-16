using MicrocontrollerSimulation.Models.Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Services.ProjectConversionServices
{
    public interface IProjectToJsonService
    {
        string Convert();
    }
}
