using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DemoDrawingContext
{
    public class VisualHostWithCollection : Panel
    {
        private IList<Page> pages;
        public Page CurrentPage { get; private set; }

        public VisualHostWithCollection()
        {
            CurrentPage = new Page();
            pages = new List<Page> { CurrentPage };
            this.MouseDown += This_MouseDown;
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

        private void This_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("here");
            //DrawingVisual path = new DrawingVisual();
            //using (var context = path.RenderOpen())
            //{
            //    context.DrawEllipse(Brushes.Black, null, e.GetPosition(this), 20, 20);
            //}
            Point mouseClickPoint = e.GetPosition(this);
            CurrentPage.DrawAt(mouseClickPoint);
        }

        protected override int VisualChildrenCount => CurrentPage.VisualChildrenCount;

        protected override Visual GetVisualChild(int index)
        {
            return CurrentPage.GetVisualChild(index);
        }
    }
}
