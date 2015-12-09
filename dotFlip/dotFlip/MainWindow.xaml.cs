using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Microsoft.Win32;

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

            _colorHistory = new Color[]
            {
                Colors.Black, Colors.White, Colors.Gray,
                Colors.Blue, Colors.Green, Colors.Red,
                Colors.Pink, Colors.Orange, Colors.Orchid
            };

            Color backgroundColor = new Color { A = 255, R = 249, G = 237, B = 78 };
            _flipbook = new Flipbook(backgroundColor);

            _flipbook.PageChanged += Flipbook_PageChanged;
            _flipbook.RefreshPage();

            _buttonsForColor = new List<Button>();
            foreach (Button b in ColorHistory.Children)
            {
                _buttonsForColor.Add(b);
            }
            UpdateNavigation();

            InitializeMenuEvents();
            BindCommands();
            
            sldrNavigation.ValueChanged += (sender, e) => _flipbook.MoveToPage(Convert.ToInt32(sldrNavigation.Value-1));
        }

        private void BindCommands()
        {
            CommandBindings.Add(new CommandBinding(ApplicationCommands.SaveAs, (sender, e) => Save()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, (sender, e) => Open()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Undo, (sender, e) => _flipbook.CurrentPage.Undo()));
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Redo, (sender, e) => _flipbook.CurrentPage.Redo()));
            CommandBindings.Add(new CommandBinding(Commands.PreviousPage, (sender, e) => _flipbook.PreviousPage()));
            CommandBindings.Add(new CommandBinding(Commands.NextPage, (sender, e) => _flipbook.NextPage()));
            CommandBindings.Add(new CommandBinding(Commands.ToggleGhostStrokes, (sender, e) =>
            {
                _flipbook.IsShowingGhostStrokes = !_flipbook.IsShowingGhostStrokes;
                bool isShowing = _flipbook.IsShowingGhostStrokes;
                string tooltip = (isShowing ? "Hide" : "Show") + " Ghost Strokes";
                btnGhost.ToolTip = tooltip;
                btnGhost.IsChecked = isShowing;
                ghostStrokesMenuItem.Header = tooltip;
            }));
            CommandBindings.Add(new CommandBinding(Commands.CopyPreviousPage, (sender, e) => _flipbook.CopyPreviousPageToCurrentPage()));
            CommandBindings.Add(new CommandBinding(Commands.ClearPage, (sender, e) => _flipbook.CurrentPage.Clear()));
            CommandBindings.Add(new CommandBinding(Commands.DeletePage, (sender, e) =>
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
            }));
            CommandBindings.Add(new CommandBinding(Commands.Restart, (sender, e) => _flipbook.DeleteAllPages()));
            CommandBindings.Add(new CommandBinding(Commands.Play, (sender, e) =>
            {
                if (_flipbook.PageCount > 1)
                {
                    _flipbook.IsPlaying = !_flipbook.IsPlaying;
                    Console.WriteLine("IsPlaying: " + _flipbook.IsPlaying);
                    chkPlay.IsChecked = _flipbook.IsPlaying;
                    if (_flipbook.IsPlaying)
                    {
                        DisableControls();
                        _flipbook.PlayAnimation(Convert.ToInt32(animationSpeedSlider.Value));                 
                    }
                    else
                    {
                        EnableControls();
                    }             
                }
            }));
            CommandBindings.Add(new CommandBinding(Commands.Export,
                (sender, e) => new ExportWindow(_flipbook).ShowDialog()));
        }

        private void InitializeMenuEvents()
        {
            sldrNavigation.ValueChanged += (sender, e) => _flipbook.MoveToPage(Convert.ToInt32(sldrNavigation.Value-1));
            toolThicknessSlider.ValueChanged += (sender, e) => { _flipbook.CurrentTool.Thickness = e.NewValue; };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateButtonColors();
            ColorButton1.Focus();
            toolThicknessSlider.Value = _flipbook.CurrentTool.Thickness;
        }

        private void Save()
        {
            var dialog = new SaveFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _flipbook.Save(dialog.FileName);
            }
        }

        private void Open()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                _flipbook.Load(dialog.FileName);
            }
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

            if (ghostPage != null)
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
            lblTotalPages.Content = _flipbook.PageCount;
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
            Control c;
        }

        public void ChangeToolColor(Color c)
        {
            Color clr = c;
            if (clr.ToString() == "#00000000")
                clr = Colors.White;
            UpdateColorHistory(clr);
            _flipbook.CurrentTool.ChangeColor(clr);
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

        private void ColorPickerbutton_Click(object sender, RoutedEventArgs e)
        {
            var clrPickerWindow = new ColorPickerWindow(this);
            clrPickerWindow.ShowDialog();
        }

        private void EnableControls()
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
            _flipbook.IsShowingGhostStrokes = false;
            flipbookHolder.IsHitTestVisible = false;
        }

        private void DisableControls()
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
        
        private void eraserButton_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Eraser");
            showAll();
            var fadeOutAnimation = new DoubleAnimation(0d, new Duration(TimeSpan.FromSeconds(1)));
            var fadeInAnimation = new DoubleAnimation(1d, new Duration(TimeSpan.FromSeconds(1)));
            eraserButton.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
            fadeTop();
            currentToolEraser.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            toolThicknessSlider.Value = _flipbook.CurrentTool.Thickness;
        }

        private void highlighterButton_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Highlighter");
            showAll();
            var fadeOutAnimation = new DoubleAnimation(0d, new Duration(TimeSpan.FromSeconds(1)));
            var fadeInAnimation = new DoubleAnimation(1d, new Duration(TimeSpan.FromSeconds(1)));
            highlighterButton.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
            fadeTop();
            currentToolHigh.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            toolThicknessSlider.Value = _flipbook.CurrentTool.Thickness;
        }


        private void Pencil_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Pencil");
            showAll();
            var fadeOutAnimation = new DoubleAnimation(0d, new Duration(TimeSpan.FromSeconds(1)));
            var fadeInAnimation = new DoubleAnimation(1d, new Duration(TimeSpan.FromSeconds(1)));
            Pencil.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
            fadeTop();
            currentToolPencil.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            toolThicknessSlider.Value = _flipbook.CurrentTool.Thickness;
        }

        private void Pen_Click(object sender, RoutedEventArgs e)
        {
            _flipbook.UseTool("Pen");
            showAll();
            var fadeOutAnimation = new DoubleAnimation(0d, new Duration(TimeSpan.FromSeconds(1)));
            var fadeInAnimation = new DoubleAnimation(1d, new Duration(TimeSpan.FromSeconds(1)));
            Pen.BeginAnimation(Image.OpacityProperty, fadeOutAnimation);
            fadeTop();
            currentToolPen.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            toolThicknessSlider.Value = _flipbook.CurrentTool.Thickness;
        }

        public void showAll()
        {
            var fadeInAnimation = new DoubleAnimation(1d, new Duration(TimeSpan.FromSeconds(1)));
            Pencil.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            Pen.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            eraserButton.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
            highlighterButton.BeginAnimation(Image.OpacityProperty, fadeInAnimation);
        }

        public void fadeTop()
        {
            var fadeoutAnimation = new DoubleAnimation(0d, new Duration(TimeSpan.FromSeconds(1)));
            currentToolPencil.BeginAnimation(Image.OpacityProperty, fadeoutAnimation);
            currentToolPen.BeginAnimation(Image.OpacityProperty, fadeoutAnimation);
            currentToolEraser.BeginAnimation(Image.OpacityProperty, fadeoutAnimation);
            currentToolHigh.BeginAnimation(Image.OpacityProperty, fadeoutAnimation);
        }
    }
}
