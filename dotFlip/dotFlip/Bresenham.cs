using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace dotFlip
{
    public class Bresenham
    {
        /// <summary>
        /// Uses Bresenham's algorithm to create many points between two points.
        /// </summary>
        /// <returns>IEnumerable of the points on the line between the two inputted points.</returns>
        public static IEnumerable<Point> GetPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool isSteep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);

            if (isSteep)
            {
                int temp = x0;
                x0 = y0;
                y0 = temp;

                temp = x1;
                x1 = y1;
                y1 = temp;
            }

            if (x0 > x1)
            {
                int temp = x0;
                x0 = x1;
                x1 = temp;

                temp = y0;
                y0 = y1;
                y1 = temp;
            }

            int dX = x1 - x0;
            int dY = Math.Abs(y1 - y0);

            int error = dX / 2;

            int yStep = (y0 < y1) ? 1 : -1;
            int y = y0;

            for (int x = x0; x <= x1; ++x)
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
            int startX = (int) a.X;
            int startY = (int) a.Y;
            int endX = (int) b.X;
            int endY = (int) b.Y;
            return GetPointsOnLine(startX, startY, endX, endY);
        }
    }
}
