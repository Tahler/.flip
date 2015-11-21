//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media;
//using System.Windows.Shapes;
//
//namespace dotFlip.Tools
//{
//    public class Eraser : ITool
//    {
//        // Interesting issue: no use for color    
//        public Color Color {get; set;}
//        public double Thickness { get; set; }
//        public Geometry GetGeometry(Point p)
//        {
//            throw new System.NotImplementedException();
//        }
//
//        private SolidColorBrush Brush { get; set; }
//
//
//        public Eraser(ref SolidColorBrush brush)
//        {
//            Brush = brush;
//            Thickness = 10;
//
//        }
//    }
//}