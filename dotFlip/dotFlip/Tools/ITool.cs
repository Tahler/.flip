using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip.Tools
{
    public interface ITool
    {
        Color Color { get; set; }
        double Thickness { get; set; }
        Shape Shape { get; }
    }
}