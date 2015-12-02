﻿using System;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Flipbook _flipbook;
        private Color[] _colorHistory;
        private List<Button> _buttonsForColor;

        public MainWindow()
        {
            InitializeComponent();
            _colorHistory = new Color[]{ Colors.White, Colors.Black, Colors.Gray, Colors.Blue, Colors.Green, Colors.Red, Colors.Pink, Colors.Orange, Colors.Orchid};
            _flipbook = new Flipbook(Colors.LightYellow);
            _flipbook.PageChanged += Flipbook_PageChanged;

            _flipbook.RefreshPage();

            _buttonsForColor = new List<Button>();
            foreach (Button b in ColorHistory.Children)
            {
                _buttonsForColor.Add(b);
            }
            UpdateNavigation();

            clearPageMenuItem.Click += (sender, e) => _flipbook.CurrentPage.Clear();
            btnNext.Click += (sender, e) => _flipbook.NextPage();
            btnPrev.Click += (sender, e) => _flipbook.PreviousPage();
            btnCopy.Click += (sender, e) => _flipbook.CopyPreviousPage();
            btnGhost.Click += (sender, e) => { _flipbook.ShowGhostStrokes = btnGhost.IsChecked.Value; _flipbook.RefreshPage(); };
            btnRedo.Click += (sender, e) => _flipbook.CurrentPage.Redo();
            btnUndo.Click += (sender, e) => _flipbook.CurrentPage.Undo();
            btnDelete.Click += (sender, e) => _flipbook.DeletePage(_flipbook.CurrentPage);
            sldrNavigation.ValueChanged += (sender, e) => _flipbook.MoveToPage(Convert.ToInt32(sldrNavigation.Value-1));
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
            Button button = sender as Button;
            if(button != null)
            {
                Rectangle rect = button.Content as Rectangle;
                if(rect != null)
                {
                    //ToolColorTester.Background = rect.Fill as SolidColorBrush;
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
            }
            _flipbook.PlayAnimation(500);
        }

    }
}
