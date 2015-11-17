using System.Windows.Controls;
using dotFlip.Tools;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip
{
    public class StickyNoteCanvas : Canvas
    {
        public ITool CurrentTool { get; set; }

        private Point previousPoint;

        public StickyNoteCanvas()
        {
            CurrentTool = new Pencil();

            MouseDown += StickyNoteCanvas_MouseDown;
            MouseMove += StickyNoteCanvas_MouseMove;
        }

        PathGeometry currentPathGeometry;
        PathFigure currentPathFigure;

        private void StickyNoteCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                previousPoint = e.GetPosition(this);

                this.Children.Add(new Path
                {
                    Stroke = CurrentTool.Brush,
                    StrokeThickness = CurrentTool.Thickness
                });

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

                //Line line = new Line
                //{
                //    Stroke = CurrentTool.Brush,
                //    StrokeThickness = CurrentTool.Thickness,
                //    X1 = previousPoint.X,
                //    Y1 = previousPoint.Y,
                //    X2 = currentPoint.X,
                //    Y2 = currentPoint.Y,
                    
                //    // potential fix for line cracks
                //    StrokeDashCap = PenLineCap.Round,
                //    StrokeStartLineCap = PenLineCap.Round,
                //    StrokeEndLineCap = PenLineCap.Round
                //};

                LineSegment segment = new LineSegment(currentPoint, true);
                currentPathFigure.Segments.Add(segment);
                currentPathGeometry.Figures.Add(currentPathFigure);
                ((Path)Children[Children.Count - 1]).Data = currentPathGeometry;

                previousPoint = currentPoint;
            }
        }
    }
}