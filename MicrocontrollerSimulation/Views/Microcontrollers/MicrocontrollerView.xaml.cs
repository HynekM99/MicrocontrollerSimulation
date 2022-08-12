using MicrocontrollerSimulation.Models.Microcontroller;
using MicrocontrollerSimulation.Models.Microcontroller.Pins;
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

namespace MicrocontrollerSimulation.Views.Microcontrollers
{
    /// <summary>
    /// Interakční logika pro MicrocontrollerView.xaml
    /// </summary>
    public partial class MicrocontrollerView : UserControl
    {
        #region SelectedPinNumberDependencyProperty
        public static readonly DependencyProperty SelectedPinNumberProperty = DependencyProperty.Register(
            nameof(SelectedPinNumber), typeof(int), typeof(MicrocontrollerView), new PropertyMetadata(-1, OnSelectedPinNumberChanged));

        public int SelectedPinNumber
        {
            get => (int)GetValue(SelectedPinNumberProperty);
            set => SetValue(SelectedPinNumberProperty, value);
        }

        private static void OnSelectedPinNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var mcu = (MicrocontrollerView)d;

            var oldPin = (int)e.OldValue;
            var newPin = (int)e.NewValue;

            if (oldPin != -1)
            {
                mcu._pinRectangles[oldPin].Style = (Style)mcu.FindResource(
                oldPin < mcu._pinRectangles.Length / 2 ?
                "TopPins" : "BottomPins");
            }

            if (newPin != -1)
            {
                mcu._pinRectangles[newPin].Style = (Style)mcu.FindResource("SelectedPin");
            }
        }
        #endregion

        public Microcontroller? Microcontroller
        {
            get => (Microcontroller?)GetValue(MicrocontrollerProperty);
            set => SetValue(MicrocontrollerProperty, value);
        }

        public static readonly DependencyProperty MicrocontrollerProperty = DependencyProperty.Register(
            nameof(Microcontroller), typeof(Microcontroller), typeof(MicrocontrollerView));

        private readonly Border[] _pinRectangles = new Border[30];
        private readonly Ellipse[] _pinEllipses = new Ellipse[30];

        public MicrocontrollerView()
        {
            InitializeComponent();

            _pinRectangles = new Border[]
            {
                rectangle0, rectangle1, rectangle2, rectangle3, rectangle4, rectangle5, rectangle6, rectangle7, rectangle8, rectangle9,
                rectangle10, rectangle11, rectangle12, rectangle13, rectangle14, rectangle15, rectangle16, rectangle17, rectangle18, rectangle19,
                rectangle20, rectangle21, rectangle22, rectangle23, rectangle24, rectangle25, rectangle26, rectangle27, rectangle28, rectangle29
            };

            _pinEllipses = new Ellipse[]
            {
                ellipse0, ellipse1, ellipse2, ellipse3, ellipse4, ellipse5, ellipse6, ellipse7, ellipse8, ellipse9,
                ellipse10, ellipse11, ellipse12, ellipse13, ellipse14, ellipse15, ellipse16, ellipse17, ellipse18, ellipse19,
                ellipse20, ellipse21, ellipse22, ellipse23, ellipse24, ellipse25, ellipse26, ellipse27, ellipse28, ellipse29
            };

            for (int i = 0; i < _pinRectangles.Length; i++)
            {
                var rectangle = _pinRectangles[i];
                var ellipse = _pinEllipses[i];

                rectangle.MouseEnter += OnRectangleMouseEnter;
                ellipse.MouseEnter += (s, e) => OnRectangleMouseEnter(rectangle, e);

                rectangle.MouseLeave += OnRectangleMouseLeave;
                ellipse.MouseLeave += (s, e) => OnRectangleMouseLeave(rectangle, e);

                rectangle.MouseLeftButtonUp += OnRectangleMouseLeftButtonUp;
                ellipse.MouseLeftButtonUp += (s, e) => OnRectangleMouseLeftButtonUp(rectangle, e);

                rectangle.KeyUp += OnRectangleKeyUp;
            }
        }

        private void OnRectangleKeyUp(object sender, KeyEventArgs e)
        {
            var rectangle = (Border)sender;
            int index = Array.IndexOf(_pinRectangles, rectangle);

            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                SelectedPinNumber = index;
            }
        }

        private void OnRectangleMouseEnter(object sender, MouseEventArgs e)
        {
            var rectangle = (Border)sender;
            rectangle.Style = (Style)FindResource("HoveredPin");
        }

        private void OnRectangleMouseLeave(object sender, MouseEventArgs e)
        {
            var rectangle = (Border)sender;
            int index = Array.IndexOf(_pinRectangles, rectangle);

            var defaultStyle = (Style)FindResource(
                index < _pinRectangles.Length / 2 ?
                "TopPins" : "BottomPins");

            rectangle.Style = SelectedPinNumber == index ? 
                (Style)FindResource("SelectedPin") : defaultStyle;
        }

        private void OnRectangleMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (Border)sender;
            int index = Array.IndexOf(_pinRectangles, rectangle);

            SelectedPinNumber = index;
        }
    }
}
