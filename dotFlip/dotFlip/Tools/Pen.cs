using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Pen : ITool
    {
        public Brush Brush { get; set; }
        public double Thickness { get; set; }

        public Geometry GetGeometry(Point p)
        {
            EllipseGeometry geometry = new EllipseGeometry(p, Thickness, Thickness);
            return geometry;
        }

        public void ChangeColor(Color c)
        {
            Brush = new SolidColorBrush(c);
        }

        public Pen()
        {
            Brush = new SolidColorBrush(Colors.Black);
            Thickness = 5;
        }

    }
}