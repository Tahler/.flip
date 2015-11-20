using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemoDrawingContext
{
    public class VisualHost : Panel
    {
        private IList<Visual> visuals;

        public VisualHost()
        {
            visuals = new List<Visual>();

            int width = (int)this.Width;
            int height = (int)this.Height;

            Random random = new Random();

            DrawingVisual path = new DrawingVisual();
            
            using (var context = path.RenderOpen())
            {
                Pen p = new Pen(Brushes.Black, 2);

                // Ellipse included for "visual debugging"
                context.DrawEllipse(Brushes.Red, p, new Point(50, 50), 20, 20);
            }
            visuals.Add(path);

            this.MouseDown += VisualHost_MouseDown;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Brush background = (Background == null) ? Brushes.Transparent : Background;
            drawingContext.DrawRectangle(background, null, new Rect(RenderSize));
        }

        private void VisualHost_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("here");
            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                Pen p = new Pen(Brushes.Black, 2);

                // Ellipse included for "visual debugging"
                context.DrawEllipse(Brushes.Red, p, e.GetPosition(this), 20, 20);
            }
            visuals.Add(path);
            AddVisualChild(path);
        }

        protected override int VisualChildrenCount => visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
    }
}
