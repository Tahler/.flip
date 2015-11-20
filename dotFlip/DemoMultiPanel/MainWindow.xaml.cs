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

namespace DemoMultiPanel
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

            previousButton.Click += PreviousButtonClick;
            nextButton.Click += NextButton_Click;

            flipbook = new Flipbook();

            flipbook.CurrentPageChanged += Flipbook_CurrentPageChanged;

            Page currentPage = flipbook.CurrentPage;
            Grid.SetColumn(currentPage, 1);
            grid.Children.Add(currentPage);
        }

        private void PreviousButtonClick(object sender, RoutedEventArgs e)
        {
            flipbook.PreviousPage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            flipbook.NextPage();
        }

        private void Flipbook_CurrentPageChanged(Page currentPage)
        {
            grid.Children.RemoveAt(1);
            Grid.SetColumn(currentPage, 1);
            grid.Children.Add(currentPage);
        }
    }
}
