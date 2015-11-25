using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using dotFlip.Tools;
using System.Threading.Tasks;

namespace dotFlip
{
    public class Page : Panel
    {
        //private IList<Visual> visuals;
        private int visibleIndex;
        private Point previousPoint;
        private bool mouseDown;
        private List<int> strokeEnd;

        private Flipbook parent;
        public bool ShowGhost { get; set; }

        public IList<Visual> Visuals { get; private set; }
        public IList<Visual> GhostVisuals { get; private set; } 

        public Page(Flipbook parent)
        {
            this.parent = parent;
            ClipToBounds = true;
            Visuals = new List<Visual>();
            GhostVisuals = new List<Visual>();
            strokeEnd = new List<int>();
            strokeEnd.Add(0);
            visibleIndex = strokeEnd.Count;


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
        private void RefreshVisibility()
        {
            if (visibleIndex != 0 && visibleIndex  <= strokeEnd.Count)
            {
                //visible
                for (int index = 0; index < strokeEnd[visibleIndex - 1]; index++)
                {
                    DrawingVisual drawVis = Visuals[index] as DrawingVisual;
                    if (drawVis != null)
                    {
                        drawVis.Opacity = 1;
                    }
                }
                //invisible
                for (int index = strokeEnd[visibleIndex - 1]; index < Visuals.Count; index++)
                {
                    DrawingVisual drawVis = Visuals[index] as DrawingVisual;
                    if (drawVis != null)
                    {
                        drawVis.Opacity = 0;
                    }
                }
            }
            else
            {
                //invisible
                for (int index = 0; index < Visuals.Count; index++)
                {
                    DrawingVisual drawVis = Visuals[index] as DrawingVisual;
                    if (drawVis != null)
                    {
                        drawVis.Opacity = 0;
                    }
                }

            }
        }
        public void Undo()
        {
            if (visibleIndex > 1)
            {
                visibleIndex--;
                RefreshVisibility();
            }
        }

        public void Redo()
        {
            if (visibleIndex < strokeEnd.Count)
            {
                visibleIndex++;
                RefreshVisibility();
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
            visibleIndex++;
            strokeEnd.Add(Visuals.Count);
            
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

        protected override int VisualChildrenCount => (ShowGhost) ? Visuals.Count + GhostVisuals.Count : Visuals.Count;

        protected override Visual GetVisualChild(int index)
        {
            if (index > Visuals.Count - 1)
            {
                return GhostVisuals[index - Visuals.Count];
            }
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
                GhostVisuals.Add(visual);
                AddVisualChild(visual);
            }
        }

        public void UpdateGhostStrokes(Page prevPage)
        {
            GhostVisuals.Clear();
            foreach (Visual v in prevPage.Visuals)
            {
                DrawingVisual visual = new DrawingVisual();
                visual.Opacity = 0.01;
                DrawingGroup group = VisualTreeHelper.GetDrawing(v);
                using (var context = visual.RenderOpen())
                {
                    context.DrawDrawing(group);
                }
                GhostVisuals.Add(visual);
                AddVisualChild(visual);
            }
        }

        public void ClearPage()
        {
            Visuals.Clear();
        }

    }
}