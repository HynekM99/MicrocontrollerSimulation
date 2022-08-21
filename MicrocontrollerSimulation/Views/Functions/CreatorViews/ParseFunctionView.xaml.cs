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

namespace MicrocontrollerSimulation.Views.Functions.CreatorViews
{
    /// <summary>
    /// Interakční logika pro ParseFunctionView.xaml
    /// </summary>
    public partial class ParseFunctionView : UserControl
    {
        public ParseFunctionView()
        {
            InitializeComponent();
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }

        private void NotButton_Click(object sender, RoutedEventArgs e)
        {
            AddSymbol("¬");
        }

        private void AndButton_Click(object sender, RoutedEventArgs e)
        {
            AddSymbol("*");
        }

        private void OrButton_Click(object sender, RoutedEventArgs e)
        {
            AddSymbol("+");
        }

        private void XorButton_Click(object sender, RoutedEventArgs e)
        {
            AddSymbol("⊕");
        }

        private void AddSymbol(string symbol)
        {
            int selectionStart = ExpressionTextBox.SelectionStart;
            ExpressionTextBox.Text = ExpressionTextBox.Text.Insert(selectionStart, symbol);
            ExpressionTextBox.Focus();
            ExpressionTextBox.SelectionStart = selectionStart + 1;
        }

        private void SurroundButton_Click(object sender, RoutedEventArgs e)
        {
            int selectionStart = ExpressionTextBox.SelectionStart;
            int selectionLength = ExpressionTextBox.SelectionLength;
            ExpressionTextBox.Text = ExpressionTextBox.Text.Insert(selectionStart, "(");
            ExpressionTextBox.Text = ExpressionTextBox.Text.Insert(selectionStart + selectionLength + 1, ")");
            ExpressionTextBox.Focus();
            ExpressionTextBox.Select(selectionStart + 1, selectionLength);
        }
    }
}
