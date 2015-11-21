using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Eraser : ITool
    {
        // Interesting issue: no use for color    
        public double Thickness { get; set; }
        public Geometry GetGeometry(Point p)
        {
            EllipseGeometry geometry = new EllipseGeometry(p, Thickness, Thickness);
            return geometry;
        }
        public Brush Brush { get; }

        public void ChangeColor(Color c)
        {
            //Eraser should not change in color - brush will change with background
        }

        public Eraser(ref SolidColorBrush brush)
        {
            Brush = brush;
            Thickness = 10;
        }
    }
}