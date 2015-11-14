using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public class Pencil : ITool
    {
        Brush brush;
        System.Windows.Media.Pen pen;

        public Color Color { get; set; }
        public double Thickness { get; set; }

        public Pencil()
        {
            // Create the Brush and Pen used for drawing.
            brush = new LinearGradientBrush(Colors.Red, Colors.Blue, 20d);
            pen = new System.Windows.Media.Pen(brush, 2d);
        }

        public void Draw(StylusPoint point, DrawingContext drawingContext)
        {
            double radius = point.PressureFactor * 10d;
            // create a bunch of small alpha-cized ellipses around point
            drawingContext.DrawEllipse(brush, pen, (Point)point, radius, radius);
        }
    }
}
