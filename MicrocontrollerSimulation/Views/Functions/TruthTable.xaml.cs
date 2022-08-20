using MicrocontrollerSimulation.Models.Functions.Base;
using MicrocontrollerSimulation.Models.LogicalExpressions.Basic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MicrocontrollerSimulation.Views.Functions
{
    /// <summary>
    /// Interakční logika pro TruthTable.xaml
    /// </summary>
    public partial class TruthTable : UserControl
    {
        #region FunctionDependencyProperty
        public Function? Function
        {
            get => (Function)GetValue(FunctionProperty);
            set => SetValue(FunctionProperty, value);
        }

        public static readonly DependencyProperty FunctionProperty = DependencyProperty.Register(nameof(Function), typeof(Function), typeof(TruthTable), new PropertyMetadata(null, OnExpressionChanged));

        private static void OnExpressionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var truthTable = (TruthTable)d;
            truthTable.ClearTable();

            if (e.NewValue is null) return;

            Function newValue = (Function)e.NewValue;
            List<Input> inputs = newValue.Expression.Inputs.ToList();

            truthTable.SetupGrid(inputs);
            truthTable.SetHeaders(inputs);
            truthTable.SetRows(inputs);
        }
        #endregion

        public TruthTable()
        {
            InitializeComponent();
        }

        private void ClearTable()
        {
            mainGrid.ColumnDefinitions.Clear();
            mainGrid.RowDefinitions.Clear();
            mainGrid.Children.Clear();
        }

        private void SetupGrid(List<Input> inputs)
        {
            int columns = inputs.Count + 2;
            int rows = (1 << inputs.Count) + 2;

            for (int i = 0; i < columns; i++)
            {
                mainGrid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = GridLength.Auto
                });
            }

            for (int i = 0; i < rows; i++)
            {
                mainGrid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = GridLength.Auto
                });
            }

            Rectangle topLine = new()
            {
                Height = 1,
                Fill = new SolidColorBrush(Colors.Gray)
            };
            Grid.SetRow(topLine, 1);
            Grid.SetColumnSpan(topLine, columns);

            Rectangle rightLine = new()
            {
                Width = 1,
                Fill = new SolidColorBrush(Colors.Gray)
            };
            Grid.SetColumn(rightLine, inputs.Count);
            Grid.SetRowSpan(rightLine, rows);

            mainGrid.Children.Add(topLine);
            mainGrid.Children.Add(rightLine);
        }

        private void SetHeaders(List<Input> inputs)
        {
            if (Function is null) return;

            for (int i = 0; i < inputs.Count; i++)
            {
                Input input = inputs[i];
                var tb = CreateTextBlock(input.ToString(), 0, i);
                
                tb.MaxWidth = 75;
                tb.TextTrimming = TextTrimming.CharacterEllipsis;
                tb.TextWrapping = TextWrapping.NoWrap;

                ToolTipService.SetToolTip(tb, input.ToString());
                ToolTipService.SetInitialShowDelay(tb, 300);

                mainGrid.Children.Add(tb);
            }
            var tb1 = CreateTextBlock("Q", 0, inputs.Count + 1);

            tb1.MaxWidth = 75;
            tb1.TextTrimming = TextTrimming.CharacterEllipsis;
            tb1.TextWrapping = TextWrapping.NoWrap;

            ToolTipService.SetToolTip(tb1, "Q");
            ToolTipService.SetInitialShowDelay(tb1, 300);

            mainGrid.Children.Add(tb1);
        }

        private void SetRows(List<Input> inputs)
        {
            if (Function is null) return;

            int columns = inputs.Count;
            int rows = 1 << inputs.Count;

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    bool bit = (i & (1 << (columns - j - 1))) > 0;

                    inputs[j].Value = bit;
                    var tb = CreateTextBlock((bit ? 1 : 0).ToString(), i + 2, j);
                    mainGrid.Children.Add(tb);
                }

                var tb1 = CreateTextBlock((Function.Expression.Result ? 1 : 0).ToString(), i + 2, columns + 1);
                mainGrid.Children.Add(tb1);
            }
        }

        private TextBlock CreateTextBlock(string text, int row, int column)
        {
            TextBlock tb = new()
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(4, 0, 4, 0)
            };
            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);

            return tb;
        }
    }
}
