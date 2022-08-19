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

            Not not = new(new Input("IN"));
            And and = new(new Input("A"), new Input("B"));
            Or or = new(new Input("A"), new Input("B"));
            Xor xor = new(new Input("A"), new Input("B"));

            var notFunction = new Function("Not", not);
            var andFunction = new Function("And", and);
            var orFunction = new Function("Or", or);
            var xorFunction = new Function("Xor", xor);

            defaultFunctions.Add(notFunction);
            defaultFunctions.Add(andFunction);
            defaultFunctions.Add(orFunction);
            defaultFunctions.Add(xorFunction);

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
