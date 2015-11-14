using System.Windows;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using dotFlip.Tools;

namespace dotFlip
{
    public class ToolRenderer : DynamicRenderer
    {
        private ITool tool;

        public ToolRenderer(ITool tool)
        {
            this.tool = tool;
        }

        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            foreach (StylusPoint t in stylusPoints)
            {
                tool.Draw(t, drawingContext);
            }
        }
    }
}