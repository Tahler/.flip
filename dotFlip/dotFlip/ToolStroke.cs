using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using dotFlip.Tools;

namespace dotFlip
{
    public class ToolStroke : Stroke
    {
        private ITool tool;

        public ToolStroke(ITool tool, StylusPointCollection stylusPoints) : base(stylusPoints)
        {
            this.tool = tool;
        }

        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            foreach (StylusPoint point in this.StylusPoints)
            {
                tool.Draw(point, drawingContext);
            }
        }
    }
}