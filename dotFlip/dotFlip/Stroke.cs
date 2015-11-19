using System.Collections;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace dotFlip
{
    public class Stroke : IEnumerable<Shape>
    {
        private IList<Shape> shapes;

        public Stroke()
        {
            shapes = new List<Shape>();
        }

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }

        public IEnumerator<Shape> GetEnumerator()
        {
            return shapes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return shapes.GetEnumerator();
        }
    }
}