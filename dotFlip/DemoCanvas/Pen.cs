using System.Windows;
using System.Windows.Media;

namespace DemoCanvas
{
    public class Pen : ITool
    {
        public Color Color { get; set; }
        public double Thickness { get; set; }
        public Brush Brush { get; }

        public Pen()
        {
            Brush = new SolidColorBrush(Colors.Black);
            Thickness = 1;
        }
    }
}