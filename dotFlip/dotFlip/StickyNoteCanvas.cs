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
    public class StickyNoteCanvas : Canvas
    {
        public ITool CurrentTool { get; private set; }
        private Dictionary<string, ITool> tools;

        private Stroke currentStroke;
        
        public StickyNoteCanvas()
        {
            tools = new Dictionary<string, ITool>
            {
                //{"Pencil", new Pencil()},
                {"Pen", new Pen()},
                {"Highlighter", new Highlighter() },
                {"Eraser", new Eraser()},
            };

            CurrentTool = tools["Pen"];
            MouseDown += StickyNoteCanvas_MouseDown;
            MouseMove += StickyNoteCanvas_MouseMove;
        }

        public void UseTool(string toolToUse)
        {
            if (tools.ContainsKey(toolToUse))
            {
                CurrentTool = tools[toolToUse];
            }
        }

        private void StickyNoteCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(this);
            currentStroke = new Stroke(point);

            if (CurrentTool is Eraser)
            {
                Erase(point);
            }
            else
            {
                Draw(point);
            }
        }

        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(this);

                // Connect points to the new point
                IEnumerable<Point> newPoints = currentStroke.ConnectTo(point);

                if (CurrentTool is Eraser)
                {
                    Erase(newPoints);
                }
                else
                {
                    Draw(newPoints);
                }
            }
        }

        private void Draw(Point point)
        {
            Shape shape = CurrentTool.Shape;
            Canvas.SetLeft(shape, point.X);
            Canvas.SetTop(shape, point.Y);
            Children.Add(shape);
        }

        private void Draw(IEnumerable<Point> pointsToBeDrawn)
        {
            foreach (Point point in pointsToBeDrawn)
            {
                Draw(point);
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

        private void Erase(IEnumerable<Point> pointsToBeErased)
        {
            if (!(CurrentTool is Eraser)) return; // if not using eraser, cannot erase

            foreach (Point point in pointsToBeErased)
            {
                Erase(point);
            }
        }
    }
}