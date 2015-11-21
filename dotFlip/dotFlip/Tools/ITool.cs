using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public interface ITool
    {
        Color Color { get; set; }
        double Thickness { get; set; }
        Geometry GetGeometry(Point p);
    }
}