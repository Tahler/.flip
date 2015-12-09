using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using dotFlip.Tools;

namespace dotFlip
{
    public class Page : Panel
    {
        private Point _previousPoint;
        private bool _mouseDown;
        private Flipbook _parent;

        private Stack<int> _undoStack; // Holds the list of indices to "rollback" to in case of an Undo() call
        private Stack<List<DrawingVisual>> _redoStack;

        public IList<DrawingVisual> Drawings { get; private set; }

        public Page(Flipbook parent)
        {
            _parent = parent;
            ClipToBounds = true;

            Background = parent.Brush;
            Drawings = new List<DrawingVisual>();

            _undoStack = new Stack<int>();
            _redoStack = new Stack<List<DrawingVisual>>();

            MouseDown += (sender, e) => Page_MouseDown(e.GetPosition(this));
            MouseMove += (sender, e) => Page_MouseMove(e.GetPosition(this));
            MouseUp += (sender, e) => Page_MouseUp(e.GetPosition(this));
            MouseLeave += (sender, e) => Page_MouseLeave(e.GetPosition(this));
        }

        public void Undo()
        {
            if (_undoStack.Count != 0)
            {
                int startIndex = Drawings.Count - 1;
                int indexToArriveAt = _undoStack.Pop();

                List<DrawingVisual> redoList = new List<DrawingVisual>();
                for (int i = startIndex; i >= indexToArriveAt; i--)
                {
                    redoList.Add(Drawings[i]);
                    Drawings.RemoveAt(i);
                }
                _redoStack.Push(redoList);

                _parent.RefreshPage();
            }
        }

        public void Redo()
        {
            if (_redoStack.Count != 0)
            {
                SaveCurrentState();

                List<DrawingVisual> redoVisuals = _redoStack.Pop();
                foreach (var visual in redoVisuals)
                {
                    Drawings.Add(visual);
                }

                _parent.RefreshPage();
            }
        }

        private void SaveCurrentState()
        {
            _undoStack.Push(Drawings.Count);
        }

        private void Page_MouseDown(Point mousePoint)
        {
            if (!_mouseDown)
            {
                _mouseDown = true;

                SaveCurrentState();
                _redoStack.Clear();

                Draw(mousePoint);
                _previousPoint = mousePoint;
            }
        }

        private void Page_MouseMove(Point mousePoint)
        {
            if (_mouseDown)
            {
                IEnumerable<Point> line = Bresenham.GetPointsOnLine(_previousPoint, mousePoint);
                foreach (var point in line) Draw(point);
                _previousPoint = mousePoint;
            }
        }

        private void Page_MouseLeave(Point mousePoint)
        {
            Page_MouseMove(mousePoint);
            Page_MouseUp(mousePoint);
        }

        private void Page_MouseUp(Point mousePoint)
        {
            _mouseDown = false;
        }

        private void Draw(Point point)
        {
            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                ITool currentTool = _parent.CurrentTool;
                context.DrawGeometry(currentTool.Brush, null, currentTool.GetGeometry(point));
            }
            Add(path);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Brush background = Background ?? Brushes.Transparent;
            drawingContext.DrawRectangle(background, null, new Rect(RenderSize));
        }

        protected override int VisualChildrenCount => Drawings.Count;

        protected override Visual GetVisualChild(int index)
        {
            return Drawings[index];
        }

        public void CopyPage(Page prevPage)
        {
            SaveCurrentState();
            foreach (var v in prevPage.Drawings)
            {
                DrawingVisual visual = new DrawingVisual();
                DrawingGroup group = VisualTreeHelper.GetDrawing(v);
                using (var context = visual.RenderOpen())
                {
                    context.DrawDrawing(group);
                }
                Add(visual);
            }
        }

        public void Clear()
        {
            SaveCurrentState();

            DrawingVisual splash = new DrawingVisual();
            using (var context = splash.RenderOpen())
            {
                context.DrawRectangle(Background, null, new Rect(this.RenderSize));
            }

            Add(splash);
        }

        public void Add(DrawingVisual visual)
        {
            Drawings.Add(visual);
            AddVisualChild(visual);
        }
    }
}