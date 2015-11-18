using System;
using System.Collections.Generic;
using System.Windows;

namespace dotFlip
{
    public class Bresenham
    {
        /// <summary>
        /// Uses Bresenham's algorithm to create many points between two points.
        /// </summary>
        /// <returns>IEnumerable of the points on the line between the two inputted points.</returns>
        public static IEnumerable<Point> GetPointsOnLine(int startX, int startY, int endX, int endY)
        {
            bool isSteep = Math.Abs(endY - startY) > Math.Abs(endX - startX);

            if (isSteep)
            {
                int temp = startX;
                startX = startY;
                startY = temp;

                temp = endX;
                endX = endY;
                endY = temp;
            }

            if (startX > endX)
            {
                int temp = startX;
                startX = endX;
                endX = temp;

                temp = startY;
                startY = endY;
                endY = temp;
            }

            int dX = endX - startX;
            int dY = Math.Abs(endY - startY);

            int error = dX / 2;

            int yStep = (startY < endY) ? 1 : -1;
            int y = startY;

            for (int x = startX; x <= endX; ++x)
            {
                yield return isSteep ? new Point(y, x) : new Point(x, y);
                error = error - dY;
                if (error < 0)
                {
                    y += yStep;
                    error += dX;
                }
            }
        }

        public static IEnumerable<Point> GetPointsOnLine(Point a, Point b)
        {
            return GetPointsOnLine((int)a.X, (int)a.Y, (int)b.X, (int)b.Y);
        }
    }
}
