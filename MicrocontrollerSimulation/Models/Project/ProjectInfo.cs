using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.Functions.Collections;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.Models.Microcontrollers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrocontrollerSimulation.Models.Project
{
    public class ProjectInfo : IDisposable
    {
        public const string DEFAULT_PROJECT_DIRECTORY = @".\projects\";
        public const string DEFAULT_PROJECT_NAME = "new_project";

        public event Action? ProjectEdited;

        public string Name { get; }
        public FunctionsCollection Functions { get; }
        public Microcontroller Microcontroller { get; }

        public ProjectInfo(string name, FunctionsCollection functions, Microcontroller microcontroller)
        {
            Name = name;
            Functions = functions;
            Microcontroller = microcontroller;

            Functions.CollectionChanged += OnFunctionsCollectionChanged;
            Microcontroller.ConfigurationChanged += OnMicrocontrollerConfigurationChanged;
        }

        private void OnMicrocontrollerConfigurationChanged()
        {
            ProjectEdited?.Invoke();
        }

        private void OnFunctionsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ProjectEdited?.Invoke();
        }

        public static ProjectInfo GetNewProject(string name = DEFAULT_PROJECT_NAME)
        {
            FunctionsCollection defaultFunctions = new();

            var not = new Not(new Input("IN"));
            var and = new And(new Input("A"), new Input("B"));
            var or = new Or(new Input("A"), new Input("B"));
            var xor = new Xor(new Input("A"), new Input("B"));

            var nand = new Not(new And(new Input("A"), new Input("B")));
            var nor = new Not(new Or(new Input("A"), new Input("B")));
            var xnor = new Not(new Xor(new Input("A"), new Input("B")));

            var notFunction = new Function("Not", not);
            var andFunction = new Function("And", and);
            var orFunction = new Function("Or", or);
            var xorFunction = new Function("Xor", xor);

            var nandFunction = new Function("Nand", nand);
            var norFunction = new Function("Nor", nor);
            var xnorFunction = new Function("Xnor", xnor);

            var srAnd = new And(new Input("Q"), new Not(new Input("R")));
            var srOr = new Or(new Input("S"), srAnd);

            var srFunction = new Function("SR_Latch", srOr);

            defaultFunctions.Add(notFunction);
            defaultFunctions.Add(andFunction);
            defaultFunctions.Add(orFunction);
            defaultFunctions.Add(xorFunction);

            defaultFunctions.Add(nandFunction);
            defaultFunctions.Add(norFunction);
            defaultFunctions.Add(xnorFunction);

            defaultFunctions.Add(srFunction);

            return new ProjectInfo(
                name,
                defaultFunctions,
                new Microcontroller());
        }

        public void Dispose()
        {
            Functions.CollectionChanged -= OnFunctionsCollectionChanged;
            Microcontroller.ConfigurationChanged -= OnMicrocontrollerConfigurationChanged;
            GC.SuppressFinalize(this);
        }
    }
}
