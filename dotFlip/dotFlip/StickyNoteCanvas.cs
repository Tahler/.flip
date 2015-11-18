using System.Windows.Controls;
using dotFlip.Tools;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using Pen = dotFlip.Tools.Pen;
using System.Collections.Generic;

namespace dotFlip
{
    public class StickyNoteCanvas : Canvas
    {
        public ITool CurrentTool { get; private set; }
        private Dictionary<string, ITool> tools;

        private Point previousPoint;

        private bool erasing;

        public StickyNoteCanvas()
        {
            tools = new Dictionary<string, ITool>
            {
                {"Pencil", new Pencil()},
                {"Pen", new Pen()},
                {"Highlighter", new Highlighter() },
                {"Eraser", new Eraser()},
            };
                
            CurrentTool = tools["Pencil"];
            MouseDown += StickyNoteCanvas_MouseDown;
            MouseMove += StickyNoteCanvas_MouseMove;
        }

        public void UseTool(string toolToUse)
        {
            erasing = (toolToUse == "Eraser");
            if (tools.ContainsKey(toolToUse))
            {
                CurrentTool = tools[toolToUse];
            }
        }

        private void StickyNoteCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(this);

            if (erasing)
            {
                Erase(point);
            }
            else
            {
                Draw(point);
            }

            previousPoint = point;
        }


        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point nextPoint = e.GetPosition(this);

                IEnumerable<Point> line = Bresenham.GetPointsOnLine(previousPoint, nextPoint);

                if (erasing)
                {
                    foreach (var point in line) Erase(point);
                }
                else
                {
                    foreach (var point in line) Draw(point);
                }

                previousPoint = nextPoint;
            }
        }

        private bool PointIsOnCanvas(Point point)
        {
            bool xOnCanvas = point.X - (CurrentTool.Thickness/2) - 3 > 0 && point.X - (CurrentTool.Thickness / 2)  < ActualWidth - 30;
            bool yOnCanvas = point.Y - (CurrentTool.Thickness/2) - 3 > 0 && point.Y - (CurrentTool.Thickness/2) < ActualHeight- 30;
            return xOnCanvas && yOnCanvas;
        }

        private void Draw(Point point)
        {
            if (PointIsOnCanvas(point))
            {
                Shape shape = CurrentTool.Shape;
                double halfThickness = CurrentTool.Thickness / 2;
                // Center the shape
                Canvas.SetLeft(shape, point.X - halfThickness);
                Canvas.SetTop(shape, point.Y - halfThickness);
                Children.Add(shape);
            }
        }

        private void Erase(Point point)
        {
            Rect eraserRect = new Rect(point, new Point(point.X + CurrentTool.Thickness, point.Y + CurrentTool.Thickness));
            for (int i = 0; i < Children.Count; i++)
            {
                Shape shape = (Shape) Children[i];

                double shapeX = Canvas.GetLeft(shape);
                double shapeY = Canvas.GetTop(shape);
                Rect shapeBounds = new Rect(new Point(shapeX, shapeY), new Point(shapeX + shape.ActualWidth, shapeY + shape.ActualHeight));

                if (eraserRect.IntersectsWith(shapeBounds))
                {
                    Children.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}