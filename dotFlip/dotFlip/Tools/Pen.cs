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
        }
    }
}