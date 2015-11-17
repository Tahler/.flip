using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

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
            }
        }

        public double Thickness { get; set; }
        public Shape Shape { get; }

        public Pencil()
        {
            //////////////////////////////////////////////////////////////////////////////////
            // Instead of a brush, use a shape now. This shape will be drawn on each point. //
            // Look at my pen (which simply uses an ellipse) for reference.                 //
            //////////////////////////////////////////////////////////////////////////////////

            Color = Colors.Gray;
            Thickness = 5;

            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Brush = Brushes.Gray;
            geometryDrawing.Pen = new System.Windows.Media.Pen(Brushes.Gray, 1);

            //GeometryGroup ellipses = new GeometryGroup();
            //ellipses.Children.Add(new RectangleGeometry(new Rect(new Point(0,45), new Point(100,89))));
            //// ellipses.Children.Add(new RectangleGeometry(new Rect(new Point(0, 100), new Point(25, 89))));

            GeometryGroup ellipses = new GeometryGroup();
            ellipses.Children.Add(new RectangleGeometry(new Rect(new Point(0, 45), new Point(100, 89))));
            // ellipses.Children.Add(new RectangleGeometry(new Rect(new Point(0, 100), new Point(25, 89))));

            geometryDrawing.Geometry = ellipses;

            // Create Brush - Brandon, this is on you to change
            DrawingBrush drawingBrush = new DrawingBrush
            {
                Drawing = geometryDrawing
            };

            //Brush = drawingBrush;
        }
    }
}
