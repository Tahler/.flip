using System.Collections.Generic;
using System.Windows.Shapes;

namespace dotFlip
{
    public class Stroke
    {
        private List<Shape> shapes;

        public Stroke()
        {
            shapes = new List<Shape>();
        }

        public void AddShape(Shape shape)
        {
            shapes.Add(shape);
        }
    }
}