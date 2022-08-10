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
    /// Interakční logika pro CreateMultiFunctionView.xaml
    /// </summary>
    public partial class CreateMultiFunctionView : UserControl
    {
        public ICommand SelectionChangedCommand
        {
            get { return (ICommand)GetValue(SelectionChangedCommandProperty); }
            set { SetValue(SelectionChangedCommandProperty, value); }
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.Register(
                nameof(SelectionChangedCommand), typeof(ICommand), typeof(CreateMultiFunctionView), new PropertyMetadata(null));

        public CreateMultiFunctionView()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItems = ((ListView)sender).SelectedItems;

            if (SelectionChangedCommand?.CanExecute(selectedItems) ?? false)
            {
                SelectionChangedCommand?.Execute(selectedItems);
            }
        }
    }
}
