using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public class Pencil : ITool
    {
        Random rand = new Random();
        private  Path p = new Path();

        private Color color;

        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                p.Fill = new SolidColorBrush(color);
            }
        }

        public double Thickness
        {
            get { return p.Width; }
            set
            {
                p.Width = value;
                p.Height = value;
            }
      
        }
        public Shape Shape
        {
            get
            {
                Path copyOfp = new Path();
                copyOfp.Data = p.Data;
                copyOfp.Fill = p.Fill;
                copyOfp.Width = p.Width;
                copyOfp.Height = p.Height;
                copyOfp.RenderTransform  = new RotateTransform(rand.NextDouble() * 360, Thickness / 2, Thickness / 2);
                return copyOfp;
            }
        }

        public Pencil()
        {
            Color = Colors.Gray;
            Thickness = 20;

            p.Data = Geometry.Parse("F1 M 0.044,25.350 C 1.034,24.289 2.506,23.486 1.986,21.820 C 3.697,20.112 5.334,18.479 6.860,16.955 C 6.860,15.665 6.860,14.694 6.860,13.913 C 8.410,11.024 11.373,10.028 13.487,8.017 C 13.778,8.282 14.127,8.601 14.627,9.057 C 17.038,7.084 20.144,8.424 22.906,7.686 C 22.651,6.448 21.324,7.340 20.995,6.460 C 20.594,5.449 21.476,5.022 21.931,4.407 C 21.548,3.908 20.821,3.622 20.964,2.806 C 21.470,0.230 24.389,1.555 25.532,0.000 C 26.580,1.048 27.640,2.108 28.904,3.372 C 29.235,5.695 29.331,8.327 30.035,10.786 C 30.793,13.429 32.121,15.909 33.249,18.577 C 31.806,20.981 30.868,25.500 31.530,28.258 C 28.797,32.106 25.646,33.671 22.926,37.500 C 20.580,38.447 17.918,38.758 15.720,40.120 C 15.174,39.611 14.815,39.276 14.463,38.948 C 14.113,39.275 13.753,39.610 13.292,40.041 C 11.761,39.414 10.198,38.764 8.901,37.686 C 8.299,38.179 7.929,38.481 7.553,38.789 C 5.528,37.068 4.552,33.480 1.864,32.758 C 0.004,32.258 -0.098,28.229 0.044,25.350 Z");
            p.Fill = Brushes.Gray;

        }
    }
}
