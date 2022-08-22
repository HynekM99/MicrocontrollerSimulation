using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MicrocontrollerSimulation.Commands
{
    public class ShowTruthTableAsyncCommand : AsyncCommandBase
    {
        private readonly FunctionsOverviewViewModel _functionsOverviewViewModel;

        public ShowTruthTableAsyncCommand(FunctionsOverviewViewModel functionsOverviewViewModel)
        {
            _functionsOverviewViewModel = functionsOverviewViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            await Task.Run(UpdateTruthTable);
        }

        private void UpdateTruthTable()
        {
            var selectedFunction = _functionsOverviewViewModel.SelectedFunction;
            if (selectedFunction is null)
            {
                _functionsOverviewViewModel.TruthTable = null;
                return;
            }

            DataTable dataTable = new();

            var inputs = selectedFunction.Expression.Inputs.ToList();

            foreach (var input in inputs)
            {
                dataTable.Columns.Add(input.Name);
            }
            dataTable.Columns.Add("{Q}");

            SetRows(inputs, dataTable);

            _functionsOverviewViewModel.TruthTable = dataTable;
        }

        private void SetRows(List<Input> inputs, DataTable dataTable)
        {
            int combinationCount = 1 << inputs.Count;

            for (int i = 0; i < combinationCount; i++)
            {
                string[] combination = new string[inputs.Count + 1];

                for (int j = 0; j < inputs.Count; j++)
                {
                    bool bit = (i & (1 << (inputs.Count - j - 1))) > 0;

                    inputs[j].Value = bit;
                    combination[j] = (bit ? 1 : 0).ToString();
                }

                combination[combination.Length - 1] = (_functionsOverviewViewModel.SelectedFunction!.Expression.Result ? 1 : 0).ToString();
                dataTable.Rows.Add(combination);
            }
        }
    }
}
