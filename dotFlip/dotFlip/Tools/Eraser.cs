using System.Windows;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public class Eraser : ITool
    {
        public double Thickness { get; set; }
        public Brush Brush { get; }

        public Eraser(ref SolidColorBrush brush)
        {
            Brush = brush;
            Thickness = 10;
        }

        public Geometry GetGeometry(Point point)
        {
            return new EllipseGeometry(point, Thickness, Thickness);
        }

        // Eraser should not change in color - brush will change with background
        public void ChangeColor(Color color) { }
    }
}