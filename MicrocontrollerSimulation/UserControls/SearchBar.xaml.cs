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

namespace MicrocontrollerSimulation.UserControls
{
    /// <summary>
    /// Interakční logika pro SearchBar.xaml
    /// </summary>
    public partial class SearchBar : UserControl
    {
        public string? SearchedExpression
        {
            get { return (string?)GetValue(SearchedExpressionProperty); }
            set { SetValue(SearchedExpressionProperty, value); }
        }

        public static readonly DependencyProperty SearchedExpressionProperty = DependencyProperty.Register(
            nameof(SearchedExpression), typeof(string), typeof(SearchBar), 
            new PropertyMetadata(null, OnSearchedExpressionChanged));

        private static void OnSearchedExpressionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)d;
            string? expression = (string?)e.NewValue;

            searchBar.TextBoxVideoName.Text = expression;
        }

        public SearchBar()
        {
            InitializeComponent();
            TextBoxVideoName.TextChanged += (s, e) => SearchedExpression = TextBoxVideoName.Text;
        }



        private void ButtonClearName_Click(object sender, RoutedEventArgs e)
        {
            TextBoxVideoName = null;
        }
    }
}
