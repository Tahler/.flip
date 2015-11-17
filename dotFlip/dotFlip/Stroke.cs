using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using dotFlip.Tools;

namespace dotFlip
{
    public class Stroke : FrameworkElement // Extending FrameworkElement does nothing right now... I would hope it can eventually be drawn as one piece.
    {
        private List<Point> points;

        public IEnumerable<Point> Points => points;

        public Stroke(Point startingPoint)
        {
            points = new List<Point> {startingPoint};
        }

        /// <summary>
        /// Uses Bresenham's algorithm to create many points between two points.
        /// </summary>
        /// <param name="point">The point to draw to</param>
        /// <returns>The newly added points.</returns>
        public IEnumerable<Point> ConnectTo(Point point)
        {
            List<Point> newPoints = new List<Point>();

            Point start = points[points.Count - 1];
            Point end = point;

            double deltaX = end.X - start.X;
            double deltaY = end.Y - start.Y;

            double error = 0;
            double deltaError = Math.Abs(deltaY/deltaX);

            int y = (int) start.Y;
            for (int x = (int)start.X;          // Start one closer than the start
                x != (int) end.X;               // Stop when at the end
                x += (deltaX > 0) ? +1 : -1)    // Get closer to the end each iteration
            {
                newPoints.Add(new Point(x, y));
                error = error + deltaError;
                while (error >= 0.5)
                {
                    newPoints.Add(new Point(x, y));
                    y += (deltaY > 0) ? +1 : -1;
                    error--;
                }
            }

            // add the newly created points to the full points
            points.AddRange(newPoints);

            // return the newly created points to be drawn
            return newPoints;
        }
    }
}