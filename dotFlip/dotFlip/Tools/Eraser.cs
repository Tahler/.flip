using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Eraser : ITool
    {
        // Interesting issue: no use for color
        public Color Color { get; set; }

        public double Thickness { get; set; }

        public Shape Shape => new Rectangle
        {
            Width = Thickness,
            Height = Thickness,
        };

        public Eraser()
        {
            Thickness = 10;
        }
    }
}