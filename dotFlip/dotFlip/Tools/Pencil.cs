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
            Color = Colors.Black;
            Thickness = 5;


            GeometryDrawing geometryDrawing = new GeometryDrawing();
            geometryDrawing.Brush = Brushes.Gray;
            geometryDrawing.Pen = new System.Windows.Media.Pen(Brushes.Gray, 1);

            GeometryGroup ellipses = new GeometryGroup();
            ellipses.Children.Add(new RectangleGeometry(new Rect(new Point(0,45), new Point(100,89))));
           // ellipses.Children.Add(new RectangleGeometry(new Rect(new Point(0, 100), new Point(25, 89))));
           
            geometryDrawing.Geometry = ellipses;



            // Create Brush - Brandon, this is on you to change
            DrawingBrush drawingBrush = new DrawingBrush
            {
                Drawing = geometryDrawing
            };

            Brush = drawingBrush;
        }
    }
}
