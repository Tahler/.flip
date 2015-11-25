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
        private Point _previousPoint;
        private bool _mouseDown;
        private Flipbook _parent;

        public bool ShowGhostStrokes { get; set; }
        private Stack<IList<Visual>> _undoStack;
        private Stack<IList<Visual>> _redoStack;

        public IList<Visual> Visuals { get; private set; }
        public IList<Visual> GhostVisuals { get; } 

        public Page(Flipbook parent)
        {
            _undoStack = new Stack<IList<Visual>>();
            _redoStack = new Stack<IList<Visual>>();
            _undoStack.Push(new List<Visual>(){});

            this._parent = parent;


            Background = parent.Brush;
            Visuals = new List<Visual>();
            GhostVisuals = new List<Visual>();



            ClipToBounds = true;

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

        public void Undo()
        {
            if (_undoStack.Count != 0)
            {
                List<Visual> tempVis = _undoStack.Pop() as List<Visual>;
                if (tempVis != null)
                {
                    Visuals = tempVis;
                    _redoStack.Push(tempVis);
                    _parent.RefreshPage();
                }
            }
        }

        public void Redo()
        {
            if (_redoStack.Count != 0)
            {
                List<Visual> tempVis = _redoStack.Pop() as List<Visual>;
                if (tempVis != null)
                {
                    Visuals = tempVis;
                    _undoStack.Push(tempVis);
                    _parent.RefreshPage();
                }
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
            List<Visual> copy = new List<Visual>();
            foreach(Visual vis in Visuals)
            {
                copy.Add(vis);
            }
            _undoStack.Push(copy);
            if(_redoStack.Count != 0)
            {
                _redoStack.Clear();
            }
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

        public void ClearPage()
        {
            Visuals.Clear();
        }
    }
}