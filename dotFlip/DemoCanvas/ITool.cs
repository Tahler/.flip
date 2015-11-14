using System.Windows.Media;

namespace DemoCanvas
{
    public interface ITool
    {
        Color Color { get; set; }
        double Thickness { get; set; }
        Brush Brush { get; }
    }
}