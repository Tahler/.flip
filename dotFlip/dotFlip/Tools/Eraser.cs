using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Eraser : ITool
    {
        // Interesting issue: no use for color    
        public Color Color {get; set;}
        private SolidColorBrush Brush { get; }

        private double _thickness;
        public double Thickness
        {
            get { return _thickness; }
            set
            {
                _thickness = value;
                _shape.Width = _thickness;
                _shape.Height = _thickness;
            }
        }

        private Ellipse _shape;
        public Shape Shape => new Ellipse()
        {
            Width = _shape.Width,
            Height = _shape.Height,
            Fill = Brush
        };

        public Eraser(ref SolidColorBrush brush)
        {
            Brush = brush;
            _thickness = 10;

            _shape = new Ellipse()
            {
                Fill = Brush,
                Width =  _thickness,
                Height = _thickness
            };
        }
    }
}