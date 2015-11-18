using System;
using System.Collections.Generic;
using System.Windows;

namespace dotFlip
{
    public class Stroke : FrameworkElement // Extending FrameworkElement does nothing right now... I would hope it can eventually be drawn as one piece.
    {
        private List<Point> points;
        public Point LastPoint => points[points.Count - 1];

        public Stroke(Point startingPoint)
        {
            points = new List<Point> {startingPoint};
        }

        public void AddPoint(Point point)
        {
            points.Add(point);
        }

        public void AddPointRange(IEnumerable<Point> points)
        {
            this.points.AddRange(points);
        }
    }
}