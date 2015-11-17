using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using dotFlip.Tools;

namespace dotFlip
{
    public class Stroke : FrameworkElement
    {
        private IList<Point> points;

        public IEnumerable<Point> Points => points;

        public Stroke(Point startingPoint)
        {
            points = new List<Point> {startingPoint};
        }

        public void ConnectTo(Point point, double error = 0)
        {
            Point start = points[points.Count - 1];
            Point end = point;

            double deltaX = end.X - start.X;
            double deltaY = end.Y - start.Y;

            double deltaError = Math.Abs(deltaY / deltaX);

            int y = (int) start.Y;
            for (int x = (int)start.X;                          // Start one closer than the start
                x != (int) end.X;                               // Stop when at the end
                x += (deltaX > 0) ? +1 : -1)                    // Get closer to the end each iteration
            {
                points.Add(new Point(x, y));
                error = error + deltaError;
                while (error >= 0.5)
                {
                    points.Add(new Point(x, y));
                    y += (deltaY > 0) ? +1 : -1;
                    error--;
                }
            }
        }
    }
}