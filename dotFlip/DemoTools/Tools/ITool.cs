using System.Windows;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public interface ITool
    {
        double Thickness { get; set; }
        Brush Brush { get; }
        Geometry GetGeometry(Point point);
        void ChangeColor(Color color);
    }
}