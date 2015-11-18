using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ColorSelector_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                Color c = (Color) ColorConverter.ConvertFromString(ColorSelector.Text);
                if (canvas != null)
                canvas.CurrentTool.Color = c;
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Color");
            }
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ThicknessSlider != null && canvas != null)
            canvas.CurrentTool.Thickness = ThicknessSlider.Value;
        }

        private void RadioPencil_Checked(object sender, RoutedEventArgs e)
        {
            var selection = sender as RadioButton;
            if (selection != null)
            {
                var title = selection.Content as string;
                if (title != null && canvas != null)
                {
                    canvas.UseTool(title);
                    ThicknessSlider.Value = canvas.CurrentTool.Thickness;
                }
            }

        }
    }
}
