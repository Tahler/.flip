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
                Shape.Fill = new SolidColorBrush(color);
            }
        }
        private double thickness;

        public double Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                Shape.Width = thickness;
                Shape.Height = thickness;
            }
        }

        public Shape Shape => new Ellipse
        {
            Fill = new SolidColorBrush(color),
            Width = thickness,
            Height = thickness,
        };

        public Pen()
        {
            color = Colors.Black;
            thickness = 5;
        }
    }
}