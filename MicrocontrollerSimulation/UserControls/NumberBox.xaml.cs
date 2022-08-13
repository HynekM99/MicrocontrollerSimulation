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
    /// Interakční logika pro NumberBox.xaml
    /// </summary>
    public partial class NumberBox : UserControl
    {
        #region ButtonsVisibleDependencyProperty
        public bool ButtonsVisible
        {
            get { return (bool)GetValue(ButtonsVisibleProperty); }
            set { SetValue(ButtonsVisibleProperty, value); }
        }

        public static readonly DependencyProperty ButtonsVisibleProperty =
            DependencyProperty.Register(nameof(ButtonsVisible), typeof(bool), typeof(NumberBox), new PropertyMetadata(true, OnButtonsVisibleChanged));

        private static void OnButtonsVisibleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumberBox nb = (NumberBox)d;
            bool visible = (bool)e.NewValue;

            nb.GridButtons.Visibility = visible ?
                Visibility.Visible :
                Visibility.Collapsed;
        }
        #endregion

        #region MinValueDependencyProperty
        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(int), typeof(NumberBox), new PropertyMetadata(int.MinValue, OnMinValueChanged, CoerceMinValue));

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumberBox nb = (NumberBox)d;

            if (nb.MinValue > nb.Value)
                nb.Value = nb.MinValue;

            nb.UpdateButtonsEnabled();
        }

        private static object CoerceMinValue(DependencyObject d, object value)
        {
            NumberBox nb = (NumberBox)d;

            if (value is int minValue)
                if (minValue > nb.MaxValue)
                    return nb.MaxValue;

            return value;
        }
        #endregion

        #region MaxValueDependencyProperty
        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(int), typeof(NumberBox), new PropertyMetadata(int.MaxValue, OnMaxValueChanged, CoerceMaxValue));

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumberBox nb = (NumberBox)d;

            if (nb.MinValue > nb.Value)
                nb.Value = nb.MinValue;

            nb.UpdateButtonsEnabled();
        }

        private static object CoerceMaxValue(DependencyObject d, object value)
        {
            NumberBox nb = (NumberBox)d;

            if (value is int maxValue)
                if (maxValue < nb.MinValue)
                    return nb.MinValue;

            return value;
        }
        #endregion

        #region ValueDependencyProperty
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(int), typeof(NumberBox),
                new PropertyMetadata(0, OnValueChanged, CoerceValue));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumberBox nb = (NumberBox)d;
            nb.TextBoxValue.Text = e.NewValue.ToString();
            nb.UpdateButtonsEnabled();
        }

        private static object CoerceValue(DependencyObject d, object value)
        {
            NumberBox nb = (NumberBox)d;
            int v = (int)value;

            if (v < nb.MinValue) return nb.MinValue;
            if (v > nb.MaxValue) return nb.MaxValue;

            return value;
        }
        #endregion

        public NumberBox()
        {
            InitializeComponent();

            MouseWheel += Border_MouseWheel;

            TextBoxValue.LostFocus += TextBox_OnLostFocus;
            TextBoxValue.PreviewKeyDown += OnTextBoxKeyDown;
            ButtonIncrement.Click += (s, e) => IncrementValue();
            ButtonDecrement.Click += (s, e) => DecrementValue();

            UpdateButtonsEnabled();
        }

        private void UpdateTextBox()
        {
            bool parsed = int.TryParse(TextBoxValue.Text, out int result);
            if (parsed) Value = result;
            TextBoxValue.Text = Value.ToString();
        }

        private void UpdateButtonsEnabled()
        {
            ButtonIncrement.IsEnabled = Value < MaxValue;
            ButtonDecrement.IsEnabled = Value > MinValue;
        }

        private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateTextBox();
            }
            else if (e.Key == Key.Escape)
            {
                TextBoxValue.Text = Value.ToString();
            }
            else if (e.Key == Key.Up || e.Key == Key.PageUp)
            {
                UpdateTextBox();
                IncrementValue();
            }
            else if (e.Key == Key.Down || e.Key == Key.PageDown)
            {
                UpdateTextBox();
                DecrementValue();
            }
        }

        private void IncrementValue()
        {
            if (Value < MaxValue) Value++;
        }

        private void DecrementValue()
        {
            if (Value > MinValue) Value--;
        }

        private void Border_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!TextBoxValue.IsFocused) return;

            UpdateTextBox();
            if (e.Delta > 0) IncrementValue();
            else if (e.Delta < 0) DecrementValue();
            e.Handled = true;
        }

        private void TextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            UpdateTextBox();
        }
    }
}
