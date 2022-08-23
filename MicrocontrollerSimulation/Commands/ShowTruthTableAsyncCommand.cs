using MicrocontrollerSimulation.Commands.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using MicrocontrollerSimulation.ViewModels.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MicrocontrollerSimulation.Commands
{
    public class ShowTruthTableAsyncCommand : AsyncCommandBase
    {
        private readonly FunctionsOverviewViewModel _functionsOverviewViewModel;

        private CancellationTokenSource? cancellationTokenSource;

        public ShowTruthTableAsyncCommand(FunctionsOverviewViewModel functionsOverviewViewModel)
        {
            _functionsOverviewViewModel = functionsOverviewViewModel;
        }

        public override async Task ExecuteAsync(object? parameter)
        {
            cancellationTokenSource = new CancellationTokenSource();
            try
            {
                await Task.Run(UpdateTruthTable, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                
            }
            finally
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }

        public void CancelExecute()
        {
            if (cancellationTokenSource is null) return;

            if (cancellationTokenSource!.Token.CanBeCanceled)
            {
                cancellationTokenSource.Cancel();
            }
        }

        private void UpdateTruthTable()
        {
            cancellationTokenSource?.Token.ThrowIfCancellationRequested();

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
            var expresion = _functionsOverviewViewModel.SelectedFunction!.Expression;

            int combinationCount = 1 << inputs.Count;

            string[] combination = new string[inputs.Count + 1];

            for (int i = 0; i < combinationCount; i++)
            {
                for (int j = 0, k = 1 << (inputs.Count - 1); j < inputs.Count; j++, k >>= 1)
                {
                    if (cancellationTokenSource!.IsCancellationRequested)
                    {
                        dataTable?.Dispose();
                        cancellationTokenSource?.Token.ThrowIfCancellationRequested();
                        return;
                    }

                    bool bit = (i & k) > 0;

                    inputs[j].Value = bit;
                    combination[j] = bit ? "1" : "0";
                }

                combination[combination.Length - 1] = expresion.Result ? "1" : "0";
                dataTable.Rows.Add(combination);
            }
        }
    }
}
