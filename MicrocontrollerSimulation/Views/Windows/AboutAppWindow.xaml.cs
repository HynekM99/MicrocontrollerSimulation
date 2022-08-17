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
    /// Interakční logika pro AboutAppWindow.xaml
    /// </summary>
    public partial class AboutAppWindow : Window
    {
        public AboutAppWindow()
        {
            InitializeComponent();
            Closing += AboutAppWindow_Closing;
        }

        private void AboutAppWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
}
