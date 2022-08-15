﻿using MicrocontrollerSimulation.JDOs.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.JDOs
{
    public class ProjectJDO
    {
        public string Name { get; set; } = string.Empty;
        public List<FunctionJDO> Functions { get; set; } = new();
    }
}
