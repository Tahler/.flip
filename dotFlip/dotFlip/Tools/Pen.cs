using System;
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

        Point previousPoint;
        public void Draw(StylusPoint point, DrawingContext drawingContext)
        {
            double radius = point.PressureFactor * Thickness;
            // fill in
            if (previousPoint != null)
            {
                double x = previousPoint.X;
                double y = previousPoint.Y;
                while (x != point.X || y != point.Y)
                {
                    
                }
            }
            drawingContext.DrawEllipse(brush, pen, (Point) point, radius, radius);

            previousPoint = (Point) point;
        }
    }
}