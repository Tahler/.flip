using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotFlip
{
    public partial class MainWindow : Window
    {
        private Flipbook flipbook;
        bool playing = false;

        public MainWindow()
        {
            InitializeComponent();
            flipbook = new Flipbook(Colors.LightYellow);
            flipbook.PageChanged += Flipbook_PageChanged;

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

        private void Flipbook_PageChanged(Page currentPage)
        {
            grid.Children.RemoveAt(1); // scary magic number :O
            Grid.SetColumn(currentPage, 1);
            grid.Children.Add(currentPage);
        }

        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.PreviousPage();
            pageNumberLabel.Content = "/" + flipbook.GetPageCount();
            pageNumberTextBox.Text = "" + flipbook.GetPageNumber(flipbook.CurrentPage);

        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            
            flipbook.NextPage();
            pageNumberLabel.Content = "/" + flipbook.GetPageCount();
            pageNumberTextBox.Text = "" + flipbook.GetPageNumber(flipbook.CurrentPage);
        }

        private void pageNumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int pageNumber = 0;
            if (int.TryParse(pageNumberTextBox.Text, out pageNumber) && flipbook != null)
            {
                flipbook.MoveToPage(pageNumber - 1);
                pageNumberLabel.Content = "/" + flipbook.GetPageCount();
                //pageNumberTextBox.Text = "" + flipbook.GetPageNumber(flipbook.CurrentPage);
            }
        }

        private void btnCopyPrevPage_Click(object sender, RoutedEventArgs e)
        {
            flipbook.CopyPrevPage();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //if (playing) {
            //    playing = true;
            //    playAnimationButton.Content = "Play";
            //}
            //else
            //{
            //    playing = true;
            //    playAnimationButton.Content = "Stop";
                flipbook.PlayAnimation();
            //}


        }
    }
}
