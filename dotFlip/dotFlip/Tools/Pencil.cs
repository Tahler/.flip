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
        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                //Brush = new SolidColorBrush(color);
            }
        }

        public double Thickness { get; set; }
        public Brush Brush { get; private set; }

        public Pencil()
        {
            Color = Colors.Gray;
            Thickness = 2;

            GeometryGroup ellipses = new GeometryGroup();
            ellipses.Children.Add(new EllipseGeometry(new Point(25, 50), 12.5, 25));
            ellipses.Children.Add(new EllipseGeometry(new Point(50, 50), 12.5, 25));
            ellipses.Children.Add(new EllipseGeometry(new Point(75, 50), 12.5, 25));

            GeometryDrawing geometryDrawing = new GeometryDrawing
            {
                Brush = Brushes.Gray,
                Pen = new System.Windows.Media.Pen(Brushes.Gray, 1),
                Geometry = ellipses,
            };

            // Create Brush - Brandon, this is on you to change
            DrawingBrush drawingBrush = new DrawingBrush
            {
                Drawing = geometryDrawing
            };

            Brush = drawingBrush;
        }
    }
}
