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

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Flipbook flipbook;

        public MainWindow()
        {
            InitializeComponent();

            flipbook = new Flipbook(Colors.LightYellow);
            flipbook.PageChanged += Flipbook_PageChanged;

            Page currentPage = flipbook.CurrentPage;
            currentPage.Width = 600;
            currentPage.Height = 600;
            flipbookHolder.Children.Add(currentPage);
        }

        private void Flipbook_PageChanged(Page currentPage)
        {
            flipbookHolder.Children.RemoveAt(0);
            flipbookHolder.Children.Add(currentPage);
        }
    }
}
