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
    /// Interakční logika pro ErrorTextBlock.xaml
    /// </summary>
    public partial class ErrorMessageBlock : UserControl
    {
        public string? ErrorMessage
        {
            get { return (string?)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }

        public static readonly DependencyProperty ErrorMessageProperty =
            DependencyProperty.Register(
                nameof(ErrorMessage), typeof(string), typeof(ErrorMessageBlock), 
                new PropertyMetadata(null, OnErrorMessageChangedCallback));

        private static void OnErrorMessageChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ErrorMessageBlock errorMessageBlock = (ErrorMessageBlock)d;
            string? errorMessage = (string?)e.NewValue;

            errorMessageBlock.textBlock.Text = errorMessage;
            errorMessageBlock.stackPanel.Visibility = errorMessage is null ? Visibility.Hidden : Visibility.Visible;
        }

        public ErrorMessageBlock()
        {
            InitializeComponent();
            textBlock.Text = null;
        }
    }
}
