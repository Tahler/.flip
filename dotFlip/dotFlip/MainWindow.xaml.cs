using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;

namespace dotFlip
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //stickyNote.DefaultDrawingAttributes = new DrawingAttributes
            //{
            //    Color = Colors.Black,
            //    FitToCurve = true,
            //};
        }
    }
    public class SliderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string redVal = "";
            string greenVal = "";
            string blueVal = "";
            for(int index = 0; index < values.Length; index++)
            {
                if(values[index] == null)
                {
                    values[index] = 0;
                }
            }
            redVal = ((int)((double)values[0])).ToString("X2");
            greenVal = ((int)((double)values[1])).ToString("X2");
            blueVal = ((int)((double)values[2])).ToString("X2");
            return ("#" + redVal + greenVal + blueVal);
           // return Color.FromArgb(255, 255, 255, 255);
           // return Color.FromArgb(255, System.Convert.ToByte(values[0]), System.Convert.ToByte(values[1]), System.Convert.ToByte(values[2]));
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
