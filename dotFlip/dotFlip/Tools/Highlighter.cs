using System.Windows;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public class Highlighter : ITool
    {
        public double Thickness { get; set; }
        public Brush Brush { get; private set; }

        public Highlighter()
        {
            Color c = Colors.Yellow;
            c.A = 12;
            Brush = new SolidColorBrush(c);
            Thickness = 5;
        }

        public Geometry GetGeometry(Point point)
        {
            EllipseGeometry geometry = new EllipseGeometry(point, Thickness, Thickness);
            return geometry;
        }

        public void ChangeColor(Color color)
        {
            color.A = 12;
            Brush = new SolidColorBrush(color);
        }
    }
}
