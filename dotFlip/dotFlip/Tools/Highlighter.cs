using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Highlighter : ITool
    {

        public Highlighter()
        {
            Color = Colors.Yellow;
            Thickness = 5;
        }

        private Color _color;

        public Color Color
        {
            get { return _color;} 
            set
            {
                _color = value;
                _color.A = 12;
            }

        }
        public double Thickness { get; set; }
        public Geometry GetGeometry(Point p)
        {
            EllipseGeometry geometry = new EllipseGeometry(p, Thickness, Thickness);
            return geometry;
        }
    }
}
