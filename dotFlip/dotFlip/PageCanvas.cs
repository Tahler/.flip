using System;
using System.Windows.Controls;
using dotFlip.Tools;
using System.Windows.Input;
using System.Windows;
using System.Windows.Shapes;
using Pen = dotFlip.Tools.Pen;
using System.Collections.Generic;
using System.Windows.Media;

namespace dotFlip
{
    public class PageCanvas : Canvas
    {
        // Undo / redo could be implemented with an undo stack of indices and a redo stack of shapes
        public ITool CurrentTool { get; private set; }
        private Dictionary<string, ITool> tools;

        private Point previousPoint;
        public Page CurrentPage { get; private set; }

        private bool mouseDown;

        public PageCanvas()
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.LightYellow);
            Background = brush;
            tools = new Dictionary<string, ITool>
            {
                {"Pencil", new Pencil()},
                {"Pen", new Pen()},
                {"Highlighter", new Highlighter()},
                {"Eraser", new Eraser(ref brush)},
            };
            
            CurrentTool = tools["Pencil"];
            MouseDown += StickyNoteCanvas_MouseDown;
            MouseMove += StickyNoteCanvas_MouseMove;
            MouseUp += StickyNoteCanvas_MouseUp;
            
        }

        //public void DisplayPage(Page page)
        //{
        //    CurrentPage = page;
        //    this.Children.Clear();
        //    foreach(Stroke stroke in page)
        //    {
        //        foreach(Shape shape in stroke)
        //        {
        //            this.Children.Add(shape);

        //        }
        //    }
        //}

        public void UseTool(string toolToUse)
        {
            if (tools.ContainsKey(toolToUse))
            {
                CurrentTool = tools[toolToUse];
            }
        }

        private void StickyNoteCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
            {
                mouseDown = true;

                Point point = e.GetPosition(this);

                Draw(point);

                previousPoint = point;
            }
        }


        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point nextPoint = e.GetPosition(this);

                IEnumerable<Point> line = Bresenham.GetPointsOnLine(previousPoint, nextPoint);
                foreach (var point in line) Draw(point);

                previousPoint = nextPoint;
            }
        }

        private void StickyNoteCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }

        private bool PointIsOnCanvas(Point point)
        {
            double halfThickness = CurrentTool.Thickness / 2;
            bool xOnCanvas = point.X - halfThickness - 3 > 0 && point.X - halfThickness < ActualWidth - 30;
            bool yOnCanvas = point.Y - halfThickness - 3 > 0 && point.Y - halfThickness < ActualHeight- 30;
            return xOnCanvas && yOnCanvas;
        }

        private void Draw(Point point)
        {
            if (PointIsOnCanvas(point))
            {
                Shape shape = CurrentTool.Shape;
                double halfThickness = CurrentTool.Thickness/2;
                // Center the shape on point
                Canvas.SetLeft(shape, point.X - halfThickness);
                Canvas.SetTop(shape, point.Y - halfThickness);
                Children.Add(shape);
            }
        }
    }
}