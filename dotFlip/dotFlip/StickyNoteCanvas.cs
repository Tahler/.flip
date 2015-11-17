using System.Windows.Controls;
using dotFlip.Tools;
using System.Windows.Input;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media;
using System.Windows.Shapes;
using Pen = dotFlip.Tools.Pen;
using System.Collections.Generic;

namespace dotFlip
{
    public class StickyNoteCanvas : Canvas
    {
        public ITool CurrentTool { get; set; }

        private Point previousPoint;
        private PathGeometry currentPathGeometry;
        private PathFigure currentPathFigure;
        private Dictionary<string, ITool> tools;
        
        public StickyNoteCanvas()
        {
            tools = new Dictionary<string, ITool>();
            tools.Add("Pencil", new Pencil());
            tools.Add("Pen", new Pen());

            CurrentTool = tools["Pen"];
            MouseDown += StickyNoteCanvas_MouseDown;
            MouseMove += StickyNoteCanvas_MouseMove;
        }

        public void useTool(string toolToUse)
        {
            if (tools.ContainsKey(toolToUse))
            {
                CurrentTool = tools[toolToUse];
            }
        }

        private void StickyNoteCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                // Set up for new path
                previousPoint = e.GetPosition(this);
                this.Children.Add(new Path());
                currentPathGeometry = new PathGeometry();
                currentPathFigure = new PathFigure {StartPoint = previousPoint};
                currentPathGeometry.Figures.Add(currentPathFigure);
            }
        }

        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPoint = e.GetPosition(this);

                // Create the new PathSegment
                LineSegment segment = new LineSegment(currentPoint, true);
                currentPathFigure.Segments.Add(segment);

                // Replace the PathFigure
                currentPathGeometry.Figures.RemoveAt(currentPathGeometry.Figures.Count - 1);
                currentPathGeometry.Figures.Add(currentPathFigure);

                // Replace the Path on the canvas
                Children.RemoveAt(Children.Count - 1);
                Children.Add(new Path
                {
                    Stroke = CurrentTool.Brush,
                    StrokeThickness = CurrentTool.Thickness,
                    Data = currentPathGeometry
                });

                previousPoint = currentPoint;
            }
        }
    }
}