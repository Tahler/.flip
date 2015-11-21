using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Pen : ITool
    {
        public Color Color { get; set; }
        public double Thickness { get; set; }

        public Geometry GetGeometry(Point p)
        {
            EllipseGeometry geometry = new EllipseGeometry(p, Thickness, Thickness);
            return geometry;
        }

        public Pen()
        {
            Color = Colors.Black;
            Thickness = 5;
        }

    }
}