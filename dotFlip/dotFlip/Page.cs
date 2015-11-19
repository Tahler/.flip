using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotFlip
{
    public class Page
    {
        private IList<Stroke> strokes;
        private Stack<Stroke> redoStack;

        public Page()
        {
            strokes = new List<Stroke>();
            redoStack = new Stack<Stroke>();
        }

        public void AddStroke(Stroke stroke)
        {
            strokes.Add(stroke);
            redoStack.Clear();
        }

        public void Undo()
        {
            int count = strokes.Count;
            if (count == 0) return;
            redoStack.Push(strokes[count - 1]);
            strokes.RemoveAt(count - 1);
        }

        public void Redo()
        {
            Stroke redoStroke = redoStack.Pop();
            strokes.Add(redoStroke);
        }
    }
}
