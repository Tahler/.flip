using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public class Pen : ITool
    {
        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                Brush = new SolidColorBrush(color);
            }
        }

        public double Thickness { get; set; }
        public Brush Brush { get; private set; }

        public Pen()
        {
            Color = Colors.Black;
            Thickness = 1;
            //Brush = new SolidColorBrush(Colors.Black);

            //////////////////// Brandon - Play with this part to make a pencil ///////////////////////////////
            //DrawingBrush drawingBrush = new DrawingBrush();

            //GeometryDrawing geometryDrawing = new GeometryDrawing();
            //geometryDrawing.Brush = Brushes.LightBlue;
            //geometryDrawing.Pen = new System.Windows.Media.Pen(Brushes.Gray, 1);

            //GeometryGroup ellipses = new GeometryGroup();
            //ellipses.Children.Add(new EllipseGeometry(new Point(25, 50), 12.5, 25));
            //ellipses.Children.Add(new EllipseGeometry(new Point(50, 50), 12.5, 25));
            //ellipses.Children.Add(new EllipseGeometry(new Point(75, 50), 12.5, 25));

            //geometryDrawing.Geometry = ellipses;

            //drawingBrush.Drawing = geometryDrawing;

            //Brush = drawingBrush;
        }
    }
}