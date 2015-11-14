using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public class Pen : ITool
    {
        private Brush brush;
        private System.Windows.Media.Pen pen;

        public Color Color { get; set; }
        public double Thickness { get; set; }

        public Pen()
        {
            brush = new SolidColorBrush(Colors.Black);
            pen = new System.Windows.Media.Pen(brush, 1);
            Thickness = 1d;
        }

        public void Draw(StylusPoint point, DrawingContext drawingContext)
        {
            double radius = point.PressureFactor * Thickness;
            drawingContext.DrawEllipse(brush, pen, (Point) point, radius, radius);
        }
    }
}