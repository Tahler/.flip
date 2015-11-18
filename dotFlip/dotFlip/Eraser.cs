using System.Windows.Media;
using System.Windows.Shapes;
using dotFlip.Tools;

namespace dotFlip
{
    public class Eraser : ITool
    {
        // Interesting issue: no use for color
        public Color Color { get; set; }

        private double thickness;
        public double Thickness
        {
            get { return thickness; }
            set
            {
                thickness = value;
                Shape = new Ellipse
                {
                    Width = thickness,
                    Height = thickness,
                };
            }
        }
        public Shape Shape { get; private set; }

        public Eraser()
        {
            Thickness = 10; // also sets shape
        }
    }
}