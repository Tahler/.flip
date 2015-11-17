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
        
        public StickyNoteCanvas()
        {
            tools = new Dictionary<string, ITool>
            {
                //{"Pencil", new Pencil()},
                {"Pen", new Pen()}
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
            previousPoint = e.GetPosition(this);
            DrawAt(e.GetPosition(this));
        }

        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawAt(e.GetPosition(this));
                previousPoint = e.GetPosition(this);
            }
        }

        Point previousPoint;
        private void DrawAt(Point point)
        {
            Stroke stroke = new Stroke(previousPoint);
            stroke.ConnectTo(point);

            foreach (Point p in stroke.Points)
            {
                Shape shape = CurrentTool.Shape;
                Canvas.SetLeft(shape, p.X);
                Canvas.SetTop(shape, p.Y);
                Children.Add(shape);
            }
        }
    }
}