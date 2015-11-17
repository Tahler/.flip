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

        private Stroke currentStroke;
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
            Point point = e.GetPosition(this);
            currentStroke = new Stroke(point);
        }

        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point point = e.GetPosition(this);
                // Connect to a new point and only draw the new points
                IEnumerable<Point> newPoints = currentStroke.ConnectTo(point);
                Draw(newPoints);
            }
        }
        
        private void Draw(IEnumerable<Point> pointsToBeDrawn)
        {
            foreach (Point p in pointsToBeDrawn)
            {
                Shape shape = CurrentTool.Shape;
                Canvas.SetLeft(shape, p.X);
                Canvas.SetTop(shape, p.Y);
                Children.Add(shape);
            }
        }
    }
}