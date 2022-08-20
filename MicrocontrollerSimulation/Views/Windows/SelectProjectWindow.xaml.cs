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
using System.Windows.Shapes;

namespace MicrocontrollerSimulation.Views.Windows
{
    /// <summary>
    /// Interakční logika pro SelectProjectWindow.xaml
    /// </summary>
    public partial class SelectProjectWindow : Window
    {
        public SelectProjectWindow()
        {
            InitializeComponent();

            Loaded += SelectProjectWindow_Loaded;
        }

        private void SelectProjectWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SearchBar.Focus();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Close();
        }
    }
}
