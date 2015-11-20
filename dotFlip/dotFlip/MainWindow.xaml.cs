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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ITool CurrentTool { get; private set; }
        private Dictionary<string, ITool> tools;

        public MainWindow()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.LightYellow);
            
            tools = new Dictionary<string, ITool>
            {
                {"Pencil", new Pencil()},
                {"Pen", new Pen()},
                {"Highlighter", new Highlighter()},
                {"Eraser", new Eraser(ref brush)},
            };
            CurrentTool = tools["Pencil"];

            InitializeComponent();
        }

        public void UseTool(string toolToUse)
        {
            if (tools.ContainsKey(toolToUse))
            {
                CurrentTool = tools[toolToUse];
            }
        }

        private void ColorSelector_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                CurrentTool.Color = (Color) ColorConverter.ConvertFromString(ColorSelector.Text);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Color");
            }
        }

        private void ThicknessSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ThicknessSlider != null) CurrentTool.Thickness = ThicknessSlider.Value;
        }

        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            var selection = sender as RadioButton;
            if (selection != null)
            {
                var title = selection.Content as string;
                if (title != null && page != null)
                {
                    UseTool(title);
                    ThicknessSlider.Value = CurrentTool.Thickness;
                    Color c = CurrentTool.Color;
                    string hexOfColor = "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
                    ColorSelector.Text = hexOfColor;
                }
            }
        }

        private void CanvasColorSelector_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (page != null && CanvasColorSelector != null)
            {
                try
                {
                    ((SolidColorBrush)page.Background).Color = (Color)ColorConverter.ConvertFromString(CanvasColorSelector.Text);

                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        //private void NextPage()
        //{
        //    int indexOfNextPage = flipBook.GetPageNumber(page.CurrentPage) + 1;
        //    MoveToPage(indexOfNextPage);
        //}

        //private void PreviousPage()
        //{
        //    int indexOfPreviousPage = flipBook.GetPageNumber(page.CurrentPage) - 1;
        //    MoveToPage(indexOfPreviousPage);
        //}

        //private void MoveToPage(int index)
        //{
        //    page.DisplayPage(flipBook.GetPageAt(index));
        //}
    }
}
