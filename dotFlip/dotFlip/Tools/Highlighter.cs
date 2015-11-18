using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    class Highlighter : ITool
    {

        public Highlighter()
        {
            Color = Colors.Yellow;
            Thickness = 5;
            Shape.Fill = new SolidColorBrush(Color);
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

        private double _thickness;
        public double Thickness { get { return _thickness; }
            set
            {
                _thickness = value;
                Shape.Height = Thickness;
                Shape.Width = Thickness;
            }
        }
        public Shape Shape => new Ellipse
        {
            Fill = new SolidColorBrush(Color),
            Width = Thickness,
            Height = Thickness,
        };
    }
}
