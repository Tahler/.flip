using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class ColorPickerWindow : Window
    {
        private MainWindow _parent;

        private Color _selectedColor;
        public ColorPickerWindow(MainWindow parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void colorPickedButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            colorPicker.SelectedColorChanged += (object s, RoutedPropertyChangedEventArgs<Color?> ev) => { _selectedColor = (Color)colorPicker.SelectedColor; };
        }

        private void ColorPickerWindow_OnClosing(object sender, CancelEventArgs e)
        {
            _parent.changeToolColor(_selectedColor);
        }
    }
}
