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
    /// Interakční logika pro NewProjectWindow.xaml
    /// </summary>
    public partial class NewProjectWindow : Window
    {
        public NewProjectWindow()
        {
            InitializeComponent();
            Loaded += NewProjectWindow_Loaded;
        }

        private void NewProjectWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxProjectName.Focus();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
