using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemoDrawingContext
{
    public class Page : Panel
    {
        private IList<Visual> visuals;

        public Page()
        {
            visuals = new List<Visual>();

            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                context.DrawEllipse(Brushes.Black, null, new Point(50, 50), 20, 20);
            }
            visuals.Add(path);
        }

        public void DrawAt(Point point) // will probably need to pass the tool
        {
            Console.WriteLine("added");
            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                context.DrawEllipse(Brushes.Black, null, point, 20, 20);
            }
            visuals.Add(path);
        }

        /// <summary>
        /// Overrides Panel's OnRender, to draw the background color or, if the background is null, a transparent rectangle. 
        /// Filling the window with this rectangle allows for Events such as MouseDown to be triggered.
        /// </summary>
        protected override void OnRender(DrawingContext drawingContext)
        {
            Brush background = Background ?? Brushes.Transparent;
            drawingContext.DrawRectangle(background, null, new Rect(RenderSize));
        }

        public new int VisualChildrenCount => visuals.Count;

        public new Visual GetVisualChild(int index)
        {
            Console.WriteLine("calling");
            return visuals[index];
        }
    }
}
