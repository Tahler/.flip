using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotFlip
{
    public partial class MainWindow : Window
    {
        private Flipbook flipbook;

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
            if (flipbook != null) { flipbook.CurrentPage.ShowGhostStrokes = false; flipbook.CurrentPage.InvalidateVisual();}
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
            grid.Children.RemoveAt(1); // scary magic number 8====================D~~~~~~~ O: 
            Grid.SetColumn(currentPage, 1);
            grid.Children.Add(currentPage);
            chkGhostStrokes.IsChecked = currentPage.ShowGhostStrokes;
        }

        private void previousPageButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.PreviousPage();
            pageNumberLabel.Content = "/" + flipbook.PageCount;
            pageNumberTextBox.Text = "" + flipbook.GetPageNumber(flipbook.CurrentPage);

        }

        private void nextPageButton_Click(object sender, RoutedEventArgs e)
        {
            
            flipbook.NextPage();
            pageNumberLabel.Content = "/" + flipbook.PageCount;
            pageNumberTextBox.Text = "" + flipbook.GetPageNumber(flipbook.CurrentPage);
        }

        private void btnCopyPrevPage_Click(object sender, RoutedEventArgs e)
        {
            flipbook.CopyPrevPage();
        }

        private void Window_ctrl(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if(e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                if(e.Key == System.Windows.Input.Key.Z)
                {
                    flipbook.CurrentPage.Undo();
                }
                else if(e.Key == System.Windows.Input.Key.Y)
                {
                    flipbook.CurrentPage.Redo();
                }
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            flipbook.CurrentPage.Undo();
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            flipbook.CurrentPage.Redo();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
                flipbook.PlayAnimation(Convert.ToInt32(animationSpeedSlider.Value));

        }
          
        private void pageNumberTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                int pageNumber = 0;
                if (int.TryParse(pageNumberTextBox.Text, out pageNumber) && flipbook != null)
                {
                    flipbook.MoveToPage(pageNumber - 1);
                    pageNumberLabel.Content = "/" + flipbook.PageCount;
                    //pageNumberTextBox.Text = "" + flipbook.GetPageNumber(flipbook.CurrentPage);
                }
            }
        }

        private void chkGhostStrokes_Click(object sender, RoutedEventArgs e)
        {
            flipbook.ToggleGhostStrokes();
            flipbook.CurrentPage.InvalidateVisual();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.CurrentPage.ClearPage();
            flipbook.RefreshPage();
        }
        
        private void deletePageButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.DeletePage(flipbook.CurrentPage);
        }
    }
}
