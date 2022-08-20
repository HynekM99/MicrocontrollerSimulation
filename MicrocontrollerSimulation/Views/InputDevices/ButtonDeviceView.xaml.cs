using MicrocontrollerSimulation.Models.InputDevices;
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

namespace MicrocontrollerSimulation.Views.InputDevices
{
    /// <summary>
    /// Interakční logika pro ButtonDeviceView.xaml
    /// </summary>
    public partial class ButtonDeviceView : UserControl
    {
        public bool IsPressed
        {
            get { return (bool)GetValue(IsPressedProperty); }
            set { SetValue(IsPressedProperty, value); }
        }

        public static readonly DependencyProperty IsPressedProperty = DependencyProperty.Register(
                nameof(IsPressed), typeof(bool), typeof(ButtonDeviceView), 
                new PropertyMetadata(false));

        public ButtonDeviceView()
        {
            InitializeComponent();

            btn.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            btn.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter ||
                e.Key == Key.Space)
            {
                SetButtonState(true);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter ||
                e.Key == Key.Space)
            {
                SetButtonState(false);
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetButtonState(false);
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SetButtonState(true);
        }

        private void SetButtonState(bool pressed)
        {
            IsPressed = pressed;
            ButtonPressedTemplate.Visibility = pressed ? Visibility.Visible : Visibility.Collapsed;
            ButtonNotPressedTemplate.Visibility = !pressed ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
