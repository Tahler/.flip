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
        private FlipBook flipBook;
        public MainWindow()
        {
            flipBook = new FlipBook();
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

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var selection = sender as RadioButton;
            if (selection != null)
            {
                var title = selection.Content as string;
                if (title != null && canvas != null)
                {
                    canvas.UseTool(title);
                    ThicknessSlider.Value = canvas.CurrentTool.Thickness;
                    Color c = canvas.CurrentTool.Color;
                    string hexOfColor = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
                    ColorSelector.Text = hexOfColor;
                }
            }
        }

        private void NextPage()
        {
            int indexOfNextPage = flipBook.GetPageNumber(canvas.CurrentPage) + 1;
            MoveToPage(indexOfNextPage);
        }

        private void PreviousPage()
        {
            int indexOfPreviousPage = flipBook.GetPageNumber(canvas.CurrentPage) - 1;
            MoveToPage(indexOfPreviousPage);
        }

        private void MoveToPage(int index)
        {
            canvas.DisplayPage(flipBook.GetPageAt(index));
        }

    }
}
