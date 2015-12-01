using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Pen : ITool
    {
        public double Thickness { get; set; }
        public Brush Brush { get; private set; }

        public Pen()
        {
            Brush = new SolidColorBrush(Colors.Black);
            Thickness = 5;
        }

        public Geometry GetGeometry(Point point)
        {
            EllipseGeometry geometry = new EllipseGeometry(point, Thickness, Thickness);
            return geometry;
        }

        public void ChangeColor(Color color)
        {
            Brush = new SolidColorBrush(color);
        }
    }
}