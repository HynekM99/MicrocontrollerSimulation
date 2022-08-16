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

namespace MicrocontrollerSimulation.Views
{
    /// <summary>
    /// Interakční logika pro SelectProjectWindow.xaml
    /// </summary>
    public partial class SelectProjectWindow : Window
    {
        public SelectProjectWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
