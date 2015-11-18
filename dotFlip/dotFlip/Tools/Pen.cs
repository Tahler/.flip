using System.Windows.Media;
using System.Windows.Shapes;

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
                shape.Fill = new SolidColorBrush(color);
            }
        }

        private double thickness;
        public double Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                shape.Width = thickness;
                shape.Height = thickness;
            }
        }

        private Ellipse shape;
        // Have to return a copy so the canvas does not get upset
        public Shape Shape => new Ellipse
        {
            Width = shape.Width,
            Height = shape.Height,
            Fill = shape.Fill
        };

        public Pen()
        {
            color = Colors.Black;
            thickness = 5;

            shape = new Ellipse
            {
                Fill = new SolidColorBrush(color),
                Width = thickness,
                Height = thickness,
            };
        }
    }
}