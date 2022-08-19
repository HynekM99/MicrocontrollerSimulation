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
    /// Interakční logika pro DecimalNumberBox.xaml
    /// </summary>
    public partial class DecimalNumberBox : UserControl
    {
        #region ValueIncrementDependencyProperty
        public double ValueIncrement
        {
            get { return (double)GetValue(ValueIncrementProperty); }
            set { SetValue(ValueIncrementProperty, value); }
        }

        public static readonly DependencyProperty ValueIncrementProperty =
            DependencyProperty.Register(nameof(ValueIncrement), typeof(double), typeof(DecimalNumberBox), new PropertyMetadata(0.1));
        #endregion

        #region ValueDecrementDependencyProperty
        public double ValueDecrement
        {
            get { return (double)GetValue(ValueDecrementProperty); }
            set { SetValue(ValueDecrementProperty, value); }
        }

        public static readonly DependencyProperty ValueDecrementProperty =
            DependencyProperty.Register(nameof(ValueDecrement), typeof(double), typeof(DecimalNumberBox), new PropertyMetadata(0.1));
        #endregion

        #region ButtonsVisibleDependencyProperty
        public bool ButtonsVisible
        {
            get { return (bool)GetValue(ButtonsVisibleProperty); }
            set { SetValue(ButtonsVisibleProperty, value); }
        }

        public static readonly DependencyProperty ButtonsVisibleProperty =
            DependencyProperty.Register(nameof(ButtonsVisible), typeof(bool), typeof(DecimalNumberBox), new PropertyMetadata(true, OnButtonsVisibleChanged));

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
        public double MinValue
        {
            get { return (double)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public static readonly DependencyProperty MinValueProperty =
            DependencyProperty.Register(nameof(MinValue), typeof(double), typeof(DecimalNumberBox), new PropertyMetadata(double.MinValue, OnMinValueChanged, CoerceMinValue));

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DecimalNumberBox nb = (DecimalNumberBox)d;

            if (nb.MinValue > nb.Value)
                nb.Value = nb.MinValue;

            nb.UpdateButtonsEnabled();
        }

        private static object CoerceMinValue(DependencyObject d, object value)
        {
            DecimalNumberBox nb = (DecimalNumberBox)d;

            if (value is int minValue)
                if (minValue > nb.MaxValue)
                    return nb.MaxValue;

            return value;
        }
        #endregion

        #region MaxValueDependencyProperty
        public double MaxValue
        {
            get { return (double)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register(nameof(MaxValue), typeof(double), typeof(DecimalNumberBox), new PropertyMetadata(double.MaxValue, OnMaxValueChanged, CoerceMaxValue));

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DecimalNumberBox nb = (DecimalNumberBox)d;

            if (nb.MinValue > nb.Value)
                nb.Value = nb.MinValue;

            nb.UpdateButtonsEnabled();
        }

        private static object CoerceMaxValue(DependencyObject d, object value)
        {
            DecimalNumberBox nb = (DecimalNumberBox)d;

            if (value is int maxValue)
                if (maxValue < nb.MinValue)
                    return nb.MinValue;

            return value;
        }
        #endregion

        #region ValueDependencyProperty
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(DecimalNumberBox),
                new PropertyMetadata(0.0, OnValueChanged, CoerceValue));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            DecimalNumberBox nb = (DecimalNumberBox)d;
            double newValue = (double)e.NewValue;
            nb.TextBoxValue.Text = newValue.ToString("N1");
            nb.UpdateButtonsEnabled();
        }

        private static object CoerceValue(DependencyObject d, object value)
        {
            DecimalNumberBox nb = (DecimalNumberBox)d;
            double v = (double)value;

            if (v < nb.MinValue) return nb.MinValue;
            if (v > nb.MaxValue) return nb.MaxValue;

            return Math.Round(v, 1, MidpointRounding.AwayFromZero);
        }
        #endregion

        public DecimalNumberBox()
        {
            InitializeComponent();

            MouseWheel += Border_MouseWheel;

            TextBoxValue.LostFocus += TextBox_OnLostFocus;
            PreviewKeyDown += OnTextBoxKeyDown;
            ButtonIncrement.Click += (s, e) => IncrementValue();
            ButtonDecrement.Click += (s, e) => DecrementValue();

            TextBoxValue.Focus();

            UpdateButtonsEnabled();
        }

        private void UpdateTextBox()
        {
            bool parsed = double.TryParse(TextBoxValue.Text, out double result);
            if (parsed) Value = result;
            TextBoxValue.Text = Value.ToString("N1");
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
            if (Value < MaxValue) Value += ValueIncrement;
        }

        private void DecrementValue()
        {
            if (Value > MinValue) Value -= ValueDecrement;
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
