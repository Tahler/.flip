using System;
using System.Globalization;
using System.Windows.Data;

namespace dotFlip
{
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
            return null;
        }
    }
}