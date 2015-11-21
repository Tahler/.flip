using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public interface ITool
    {
        Brush Brush { get; set; }
        double Thickness { get; set; }
        Geometry GetGeometry(Point p);
        void ChangeColor(Color c);
    }
}