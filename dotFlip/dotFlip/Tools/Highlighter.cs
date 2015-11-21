using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Highlighter : ITool
    {

        public Highlighter()
        {
            Color c = Colors.Yellow;
            c.A = 12;
            Brush = new SolidColorBrush(c);
            Thickness = 5;
        }

        public Brush Brush { get; private set; }
        public double Thickness { get; set; }
        public Geometry GetGeometry(Point p)
        {
            EllipseGeometry geometry = new EllipseGeometry(p, Thickness, Thickness);
            return geometry;
        }

        public void ChangeColor(Color c)
        {
            c.A = 12;
            Brush = new SolidColorBrush(c);
        }
    }
}
