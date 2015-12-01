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
using Xceed.Wpf.Toolkit;

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Color[] _colorHistory;
        private List<Button> _buttonsForColor;
        public MainWindow()
        {
            InitializeComponent();
            _colorHistory = new Color[]{ Colors.White, Colors.White, Colors.White, Colors.White, Colors.White, Colors.White, Colors.White, Colors.White};
            _buttonsForColor = new List<Button>();
            foreach(Button b in ColorHistory.Children)
            {
                _buttonsForColor.Add(b);
            }
        }

        private void UpdateColorHistory(Color c)
        {
            for(int index = 7; index > 0; index--)
            {
                _colorHistory[index] = _colorHistory[index - 1];
            }
            _colorHistory[0] = c;
            UpdateButtonColors();
        }

        private void UpdateButtonColors()
        {
            int index = 0;
            foreach(Button b in _buttonsForColor)
            {
                Rectangle rect = b.Content as Rectangle;
                if(rect != null)
                {
                    rect.Fill = new SolidColorBrush(_colorHistory[index]);
                }
                index++;
            }
            //Rectangle innerButton = ColorButton1.Content as Rectangle;
            //if (innerButton != null)
            //{
            //    Color c = _colorHistory[0];
            //    innerButton.Fill = new SolidColorBrush(c);
            //}
        }

        private void ToolClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = (Color)e.NewValue;
            Brush backgroundColor = new SolidColorBrush(color);

            ToolColorTester.Background = backgroundColor;

            UpdateColorHistory(color);

        }

        private void StickyNoteClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = (Color)e.NewValue;
            Brush backgroundColor = new SolidColorBrush(color);

            StickyNoteColorTester.Background = backgroundColor;

        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            if(but != null)
            {
                Rectangle rect = but.Content as Rectangle;
                if(rect != null)
                {
                    SolidColorBrush b = rect.Fill as SolidColorBrush;
                    Tool_ClrPicker.SelectedColor = b.Color;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtPageNum.Text = "6";
        }
    }
}
