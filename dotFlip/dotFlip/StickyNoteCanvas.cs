using System.Windows.Controls;
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;

namespace dotFlip
{
    public class StickyNoteCanvas : InkCanvas
    {
        private ITool currentTool;

        public StickyNoteCanvas()
        {
            UseTool(new Pen());
        }

        public void UseTool(ITool tool)
        {
            currentTool = tool;
            this.DynamicRenderer = new ToolRenderer(tool);
        }

        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            // Remove the original stroke and add the custom stroke.
            this.Strokes.Remove(e.Stroke);
            ToolStroke stroke = new ToolStroke(currentTool, e.Stroke.StylusPoints);
            this.Strokes.Add(stroke);

            // Pass the custom stroke to base class' OnStrokeCollected method.
            InkCanvasStrokeCollectedEventArgs args = new InkCanvasStrokeCollectedEventArgs(stroke);
            base.OnStrokeCollected(args);
        }
    }
}