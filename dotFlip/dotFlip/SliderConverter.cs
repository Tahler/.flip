using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace dotFlip
{
    public class SliderConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            for (int index = 0; index < values.Length; index++)
            {
                if (values[index] == null)
                {
                    values[index] = 0;
                }
            }

            int red = System.Convert.ToInt32(values[0]);
            string redVal = red.ToString("X2");

            int green = System.Convert.ToInt32(values[1]); 
            string greenVal = green.ToString("X2");

            int blue = System.Convert.ToInt32(values[2]); ;
            string blueVal = blue.ToString("X2");
            return ("#" + redVal + greenVal + blueVal);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            Color c = new Color();
            object[] val = null;
            try
            {
                var convertFromString = ColorConverter.ConvertFromString((string) value);
                if (convertFromString != null) c = (Color) convertFromString;
                val = new object[] { System.Convert.ToDouble(c.R), System.Convert.ToDouble(c.G), System.Convert.ToDouble(c.B) };
            }
            catch (FormatException)
            {
                // ignored
            }
            
            return val;
        }
    }
}