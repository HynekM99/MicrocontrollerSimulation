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
    /// Interakční logika pro SwitchDeviceView.xaml
    /// </summary>
    public partial class SwitchDeviceView : UserControl
    {
        public bool IsToggled
        {
            get { return (bool)GetValue(IsToggledProperty); }
            set { SetValue(IsToggledProperty, value); }
        }

        public static readonly DependencyProperty IsToggledProperty = DependencyProperty.Register(
                nameof(IsToggled), typeof(bool), typeof(SwitchDeviceView),
                new PropertyMetadata(false, OnIsToggledChanged));

        private static void OnIsToggledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SwitchDeviceView view = (SwitchDeviceView)d;
            bool toggled = (bool)e.NewValue;
            view.SwitchOnTemplate.Visibility = toggled ? Visibility.Visible : Visibility.Collapsed;
            view.SwitchOffTemplate.Visibility = !toggled ? Visibility.Visible : Visibility.Collapsed;
        }

        public SwitchDeviceView()
        {
            InitializeComponent();

            MainButton.Click += MainButton_Click;
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleSwitchState();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter ||
                e.Key == Key.Space)
            {
                ToggleSwitchState();
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter ||
                e.Key == Key.Space)
            {
                ToggleSwitchState();
            }
        }

        private void ToggleSwitchState()
        {
            IsToggled = !IsToggled;
        }
    }
}
