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
            
            this.MouseDown += VisualHost_MouseDown;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Brush background = (Background == null) ? Brushes.Transparent : Background;
            drawingContext.DrawRectangle(background, null, new Rect(RenderSize));
        }

        private void VisualHost_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                context.DrawEllipse(Brushes.Black, null, e.GetPosition(this), 20, 20);
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
