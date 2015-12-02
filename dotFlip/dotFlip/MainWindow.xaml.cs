using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace dotFlip
{
    public partial class MainWindow : Window
    {
        // http://stackoverflow.com/questions/1361350/keyboard-shortcuts-in-wpf

        private Flipbook _flipbook;
        private Color[] _colorHistory;
        private List<Button> _buttonsForColor;

        public MainWindow()
        {
            InitializeComponent();
            _colorHistory = new Color[]{ Colors.Black, Colors.White, Colors.Gray, Colors.Blue, Colors.Green, Colors.Red, Colors.Pink, Colors.Orange, Colors.Orchid};
            _flipbook = new Flipbook(Colors.LightYellow);
            _flipbook.PageChanged += Flipbook_PageChanged;

            _flipbook.RefreshPage();

            _buttonsForColor = new List<Button>();
            foreach (Button b in ColorHistory.Children)
            {
                _buttonsForColor.Add(b);
            }
            UpdateNavigation();

            InitializeMenuItemClickEvents();
            InitializeButtonClickEvents();
            BindCommands();
            
            sldrNavigation.ValueChanged += (sender, e) => _flipbook.MoveToPage(Convert.ToInt32(sldrNavigation.Value-1));
        }

        private void BindCommands()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, (sender, e) => _flipbook.CurrentPage.Undo()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, (sender, e) => _flipbook.CurrentPage.Redo()));
            CommandBindings.Add(new CommandBinding(Commands.PreviousPage, (sender, e) => _flipbook.PreviousPage()));
            CommandBindings.Add(new CommandBinding(Commands.NextPage, (sender, e) => _flipbook.NextPage()));
            CommandBindings.Add(new CommandBinding(Commands.ShowGhostStrokes, (sender, e) => _flipbook.ShowGhostStrokes = !_flipbook.ShowGhostStrokes));
            CommandBindings.Add(new CommandBinding(Commands.CopyPreviousPage, (sender, e) => _flipbook.CopyPreviousPageToCurrentPage()));
            CommandBindings.Add(new CommandBinding(Commands.ClearPage, (sender, e) => _flipbook.CurrentPage.Clear()));
            CommandBindings.Add(new CommandBinding(Commands.DeletePage, (sender, e) => _flipbook.DeletePage(_flipbook.CurrentPage)));
            CommandBindings.Add(new CommandBinding(Commands.Restart, (sender, e) => _flipbook.DeleteAllPages()));
        }

        private void InitializeButtonClickEvents()
        {
            btnNext.Click += (sender, e) => _flipbook.NextPage();
            btnPrev.Click += (sender, e) => _flipbook.PreviousPage();
            btnCopy.Click += (sender, e) => _flipbook.CopyPreviousPageToCurrentPage();
            btnGhost.Click += (sender, e) => _flipbook.ShowGhostStrokes = btnGhost.IsChecked.Value;
            btnRedo.Click += (sender, e) => _flipbook.CurrentPage.Redo();
            btnUndo.Click += (sender, e) => _flipbook.CurrentPage.Undo();
        }

        private void InitializeMenuItemClickEvents()
        {
            btnDelete.Click += (sender, e) =>
            {
                Point lowerRightPoint = this.PointToScreen(new Point(0, 0));
                lowerRightPoint.X += this.ActualWidth;
                lowerRightPoint.Y += this.ActualHeight - StatusBar.ActualHeight;
                Notification note = new Notification("Delete Page", "Are you sure you want to delete this page?", lowerRightPoint);
                var showDialog = note.ShowDialog();
                Console.WriteLine(showDialog.Value);
                if (showDialog.Value)
                {
                    _flipbook.DeletePage(_flipbook.CurrentPage);
                }
            };
            sldrNavigation.ValueChanged += (sender, e) => _flipbook.MoveToPage(Convert.ToInt32(sldrNavigation.Value-1));
            toolThicknessSlider.ValueChanged += (sender, e) => { _flipbook.CurrentTool.Thickness = e.NewValue; };
        }

        private void Flipbook_PageChanged(Page currentPage, Page ghostPage)
        {
            flipbookHolder.Children.Clear();

            // Readd the border, since clearing removes it
            flipbookHolder.Children.Add(new Border
            {
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(10),
                Margin = new Thickness(-1),
                Effect = new DropShadowEffect
                {
                    Color = Colors.Black,
                }
            });

            currentPage.Opacity = 1;
            currentPage.IsHitTestVisible = true;
            flipbookHolder.Children.Add(currentPage);

            if (ghostPage != null && _flipbook.ShowGhostStrokes)
            {
                ghostPage.Opacity = 0.05;
                ghostPage.IsHitTestVisible = false;
                flipbookHolder.Children.Add(ghostPage);
            }
            UpdateNavigation();
        }

        private void UpdateNavigation()
        {
            sldrNavigation.Maximum = _flipbook.PageCount;
            sldrNavigation.IsEnabled = _flipbook.PageCount > 1 && !_flipbook.IsPlaying;
            chkPlay.IsEnabled = _flipbook.PageCount > 1;
            sldrNavigation.Value = _flipbook.GetPageNumber(_flipbook.CurrentPage);
            lblTotalPages.Content = "of " + _flipbook.PageCount;
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
            foreach(Button button in _buttonsForColor)
            {
                Rectangle rect = button.Template.FindName("ColorHistoryRectangle", button) as Rectangle;
                if (rect != null)
                {
                    rect.Fill = new SolidColorBrush(_colorHistory[index]);
                }
                //Rectangle rect = b.Content as Rectangle;
                //if(rect != null)
                //{
                //    rect.Fill = new SolidColorBrush(_colorHistory[index]);
                //}
                index++;
            }
            //Rectangle innerButton = ColorButton1.Content as Rectangle;
            //if (innerButton != null)
            //{
            //    Color c = _colorHistory[0];
            //    innerButton.Fill = new SolidColorBrush(c);
            //}
        }

        public void changeToolColor(Color c)
        {
            UpdateColorHistory(c);
            _flipbook.CurrentTool.ChangeColor(c);
            ColorButton1.Focus();
        }

        private void StickyNoteClrPcker_Background_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            _flipbook.BackgroundColor = (Color)e.NewValue;
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if(button != null)
            {
                Rectangle rect = button.Template.FindName("ColorHistoryRectangle", button) as Rectangle;
                if(rect != null)
                {
                    SolidColorBrush rectColor = rect.Fill as SolidColorBrush;
                    _flipbook.CurrentTool.ChangeColor(rectColor.Color);
                }
            //    Rectangle rect = button.Content as Rectangle;
            //    if(rect != null)
            //    {
            //        SolidColorBrush rectColor = rect.Fill as SolidColorBrush;
            //        _flipbook.CurrentTool.ChangeColor(rectColor.Color);
            //    }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtonColors();
            ColorButton1.Focus();
        }

        private void ColorPickerbutton_Click(object sender, RoutedEventArgs e)
        {
            ColorPickerWindow clrPickerWindow;
            clrPickerWindow = new ColorPickerWindow(this);
            clrPickerWindow.Show();
        }

        private void Pencil_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Pencil");
            currentToolImage.Source = new BitmapImage(new Uri("pack://application:,,,/Image/pencil.png"));
        }

        private void Pen_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Pen");
            currentToolImage.Source = new BitmapImage(new Uri("pack://application:,,,/Image/pen.png"));
        }

        private void chkPlay_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.IsPlaying = chkPlay.IsChecked.Value;
            if (chkPlay.IsChecked.Value)
            {
                btnNext.IsEnabled = false;
                btnPrev.IsEnabled = false;
                btnUndo.IsEnabled = false;
                btnUndo.IsEnabled = false;
                btnCopy.IsEnabled = false;
                btnRedo.IsEnabled = false;
                btnDelete.IsEnabled = false;
                txtNavigation.IsEnabled = false;
                sldrNavigation.IsEnabled = false;
                btnGhost.IsChecked = false;
                btnGhost.IsEnabled = false;
                _flipbook.ShowGhostStrokes = false;
                flipbookHolder.IsHitTestVisible = false;
            }
            else
            {
                btnNext.IsEnabled = true;
                btnPrev.IsEnabled = true;
                btnUndo.IsEnabled = true;
                btnUndo.IsEnabled = true;
                btnCopy.IsEnabled = true;
                btnDelete.IsEnabled = true;
                txtNavigation.IsEnabled = true;
                sldrNavigation.IsEnabled = true;
                btnRedo.IsEnabled = true;
                btnGhost.IsEnabled = true;
                flipbookHolder.IsHitTestVisible = true;
            }
            _flipbook.PlayAnimation(500);
        }
        private void eraserButton_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Eraser");
            currentToolImage.Source = new BitmapImage(new Uri("pack://application:,,,/Image/eraser.png"));
        }

        private void highlighterButton_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Highlighter");
            currentToolImage.Source = new BitmapImage(new Uri("pack://application:,,,/Image/high.png"));
        }
    }
}
