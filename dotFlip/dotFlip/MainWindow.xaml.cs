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
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;

namespace dotFlip
{
    public partial class MainWindow : Window
    {
        private Flipbook flipbook;

        public MainWindow()
        {
            InitializeComponent();
            flipbook = new Flipbook(Colors.LightYellow);
            flipbook.CurrentPageChanged += Flipbook_CurrentPageChanged;

            Page currentPage = flipbook.CurrentPage;
            Grid.SetColumn(currentPage, 1);
            grid.Children.Add(currentPage);
        }

        private void ColorSelector_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (flipbook != null)
                {
                    var convertFromString = ColorConverter.ConvertFromString(ColorSelector.Text);
                    if (convertFromString != null) flipbook.CurrentTool.ChangeColor((Color)convertFromString);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Color");
            }
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ThicknessSlider != null && flipbook != null) flipbook.CurrentTool.Thickness = ThicknessSlider.Value;
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var selection = sender as RadioButton;
            if (selection != null)
            {
                var title = selection.Content as string;
                if (title != null && flipbook != null)
                {
                    flipbook.UseTool(title);
                    ThicknessSlider.Value = flipbook.CurrentTool.Thickness;
                    Color c = ((SolidColorBrush) flipbook.CurrentTool.Brush).Color;
                    string hexOfColor = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
                    ColorSelector.Text = hexOfColor;
                }
            }
        }

        private void CanvasColorSelector_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CanvasColorSelector != null)
            {
                if (flipbook != null)
                {
                    var convertFromString = ColorConverter.ConvertFromString(CanvasColorSelector.Text);
                    if (convertFromString != null) flipbook.BackgroundColor = (Color)convertFromString;
                }
            }
        }

        private void Flipbook_CurrentPageChanged(Page currentPage)
        {
            grid.Children.RemoveAt(1); // scary magic number :O
            Grid.SetColumn(currentPage, 1);
            grid.Children.Add(currentPage);
        }

        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.PreviousPage();
        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.NextPage();
        }
    }
}
