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
            DrawAt(e.GetPosition(this));
        }

        private void StickyNoteCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawAt(e.GetPosition(this));
            }
        }

        private void DrawAt(Point point)
        {
            Shape shape = CurrentTool.Shape;
            Canvas.SetLeft(shape, point.X);
            Canvas.SetTop(shape, point.Y);
            Children.Add(shape);
        }
    }
}