using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using dotFlip.Tools;

namespace dotFlip
{
    public class Page : Panel
    {
        //private IList<Visual> visuals;

        private Point previousPoint;
        private bool mouseDown;

        private Flipbook parent;

        public IList<Visual> Visuals { get; private set; }

        public Page(Flipbook parent)
        {
            this.parent = parent;
            ClipToBounds = true;
            Visuals = new List<Visual>();

            Background = parent.Brush;

            MouseDown += Page_MouseDown;
            MouseMove += Page_MouseMove;
            MouseUp += Page_MouseUp;
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!mouseDown)
            {
                mouseDown = true;

                Point point = e.GetPosition(this);
                Draw(point);

                previousPoint = point;
            }
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseDown)
            {
                Point nextPoint = e.GetPosition(this);

                IEnumerable<Point> line = Bresenham.GetPointsOnLine(previousPoint, nextPoint);
                foreach (var point in line) Draw(point);

                previousPoint = nextPoint;
            }
        }

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            mouseDown = false;
        }

        private void Draw(Point point)
        {          
                DrawingVisual path = new DrawingVisual();
                using (var context = path.RenderOpen())
                {
                    ITool currentTool = parent.CurrentTool;
                    context.DrawGeometry(currentTool.Brush, null, currentTool.GetGeometry(point));
                }
                Visuals.Add(path);
                AddVisualChild(path);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Brush background = Background ?? Brushes.Transparent;
            drawingContext.DrawRectangle(background, null, new Rect(RenderSize));
        }

        protected override int VisualChildrenCount => Visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            return Visuals[index];
        }

        public void CopyPage(Page prevPage)
        {
            foreach (Visual v in prevPage.Visuals)
            {
                DrawingVisual visual = new DrawingVisual();
                DrawingGroup group = VisualTreeHelper.GetDrawing(v);
                using (var context = visual.RenderOpen())
                {
                    context.DrawDrawing(group);
                }
                Visuals.Add(visual);
                AddVisualChild(visual);
            }
            InvalidateVisual();
        }

    }
}