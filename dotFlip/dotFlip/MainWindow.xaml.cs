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
        private Flipbook flipbook;
        private Color[] _colorHistory;
        private List<Button> _buttonsForColor;

        public MainWindow()
        {
            InitializeComponent();
            _colorHistory = new Color[]{ Colors.White, Colors.Black, Colors.Gray, Colors.Blue, Colors.Green, Colors.Red, Colors.Pink, Colors.Orange, Colors.Orchid};
            flipbook = new Flipbook(Colors.LightYellow);
            flipbook.PageChanged += Flipbook_PageChanged;

            Page currentPage = flipbook.CurrentPage;
            flipbookHolder.Children.Add(currentPage);

            _buttonsForColor = new List<Button>();
            foreach (Button b in ColorHistory.Children)
            {
                _buttonsForColor.Add(b);
            }
        }

        private void Flipbook_PageChanged(Page currentPage)
        {
            flipbookHolder.Children.Clear();
            flipbookHolder.Children.Add(currentPage);
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

            UpdateColorHistory(color);

        }

        private void StickyNoteClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            Color color = (Color)e.NewValue;
            Brush backgroundColor = new SolidColorBrush(color);
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button but = sender as Button;
            if(but != null)
            {
                Rectangle rect = but.Content as Rectangle;
                if(rect != null)
                {
                    ToolColorTester.Background = rect.Fill as SolidColorBrush;
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtonColors();
        }

        private void ColorPickerbutton_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerWindow clrPickerWindow = new ColorPickerWindow();
            clrPickerWindow.Show();
        }

        private void Pencil_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnNext_OnClick(object sender, RoutedEventArgs e)
        {
            flipbook.NextPage();
        }

        private void BtnPrev_OnClick(object sender, RoutedEventArgs e)
        {
            flipbook.PreviousPage();
        }

        private void BtnUndo_OnClick(object sender, RoutedEventArgs e)
        {
            flipbook.CurrentPage.Undo();
        }

        private void BtnRedo_OnClick(object sender, RoutedEventArgs e)
        {
            flipbook.CurrentPage.Redo();
        }

        private void BtnCopy_OnClick(object sender, RoutedEventArgs e)
        {
            flipbook.CopyPreviousPage();
        }

        private void BtnGhost_OnClick(object sender, RoutedEventArgs e)
        {
            //Replace with dependancy property
            flipbook.ShowGhostStrokes = btnGhost.IsChecked.Value;
            flipbook.RefreshPage();
        }

        private void BtnDelete_OnClick(object sender, RoutedEventArgs e)
        {
            flipbook.DeletePage(flipbook.CurrentPage);
        }
    }
}
