using System.Windows.Input;
using System.Windows.Media;

namespace dotFlip.Tools
{
    public interface ITool
    {
        Color Color { get; set; }
        double Thickness { get; set; }
        Brush Brush { get; }
    }
}