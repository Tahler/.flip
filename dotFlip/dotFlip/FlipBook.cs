using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace dotFlip
{
    public class Flipbook : Panel
    {
        private IList<Page> pages;
        public Page CurrentPage { get; private set; }

        public Flipbook()
        {
            CurrentPage = new Page();
            pages = new List<Page> {CurrentPage};
            this.MouseDown += Flipbook_MouseDown;
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

        private void Flipbook_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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

        protected override int VisualChildrenCount => CurrentPage.VisualCount;

        protected override Visual GetVisualChild(int index)
        {
            Console.WriteLine("calling");
            return CurrentPage[index];
        }

        //public void AddPage(Page p)
        //{
        //    pages.Add(p);
        //}

        //public void RemovePage(Page p)
        //{
        //    pages.Remove(p);
        //}

        //public void RemoveAt(int index)
        //{
        //    pages.RemoveAt(index);
        //}

        //public Page GetPageAt(int index)
        //{
        //    return pages[index];
        //}

        //public int GetPageNumber(Page p)
        //{
        //    return pages.IndexOf(p);
        //}
    }
}
