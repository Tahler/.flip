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
        private int _visibleIndex;
        private Point _previousPoint;
        private bool _mouseDown;
        private List<int> _strokeEnd;
        private Flipbook _parent;

        public bool ShowGhostStrokes { get; set; }

        public IList<Visual> Visuals { get; }
        public IList<Visual> GhostVisuals { get; } 

        public Page(Flipbook parent)
        {
            _parent = parent;
            ClipToBounds = true;

            Background = parent.Brush;
            Visuals = new List<Visual>();
            GhostVisuals = new List<Visual>();

            _strokeEnd = new List<int> {0};
            _visibleIndex = _strokeEnd.Count;

            MouseDown += Page_MouseDown;
            MouseMove += Page_MouseMove;
            MouseUp += Page_MouseUp;
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!_mouseDown)
            {
                _mouseDown = true;
                Point point = e.GetPosition(this);
                Draw(point);
                _previousPoint = point;
            }
        }
        private void RefreshVisibility()
        {
            if (_visibleIndex != 0 && _visibleIndex  <= _strokeEnd.Count)
            {
                // Visible
                for (int index = 0; index < _strokeEnd[_visibleIndex - 1]; index++)
                {
                    DrawingVisual drawVis = Visuals[index] as DrawingVisual;
                    if (drawVis != null)
                    {
                        drawVis.Opacity = 1;
                    }
                }
                // Invisible
                for (int index = _strokeEnd[_visibleIndex - 1]; index < Visuals.Count; index++)
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
                // Invisible
                foreach (Visual t in Visuals)
                {
                    DrawingVisual drawVis = t as DrawingVisual;
                    if (drawVis != null)
                    {
                        drawVis.Opacity = 0;
                    }
                }

            }
        }

        public void Undo()
        {
            if (_visibleIndex > 1)
            {
                _visibleIndex--;
                RefreshVisibility();
            }
        }
        public void Redo()
        {
            if (_visibleIndex < _strokeEnd.Count)
            {
                _visibleIndex++;
                RefreshVisibility();
            }
        }

        private void Page_MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
            {
                Point nextPoint = e.GetPosition(this);
                IEnumerable<Point> line = Bresenham.GetPointsOnLine(_previousPoint, nextPoint);
                foreach (var point in line) Draw(point);
                _previousPoint = nextPoint;
            }
        }

        private void Page_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
            _visibleIndex++;
            _strokeEnd.Add(Visuals.Count);
        }

        private void Draw(Point point)
        {
            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                ITool currentTool = _parent.CurrentTool;
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

        protected override int VisualChildrenCount => (ShowGhostStrokes) ? Visuals.Count + GhostVisuals.Count : Visuals.Count;

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
    }
}