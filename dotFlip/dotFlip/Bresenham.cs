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
        //public static IEnumerable<Point> GetPointsOnLine(int startX, int startY, int endX, int endY)
        //{
        //    bool isSteep = Math.Abs(endY - startY) > Math.Abs(endX - startX);

        //    if (isSteep)
        //    {
        //        // Swap startX and startY
        //        int temp = startX;
        //        startX = startY;
        //        startY = temp;
        //        // Swap endX and endY 
        //        temp = endX;
        //        endX = endY;
        //        endY = temp;
        //    }
        //    if (startX > endX)
        //    {
        //        // Swap startX and endX
        //        int temp = startX;
        //        startX = endX;
        //        endX = temp;
        //        // Swap startY and endY
        //        temp = startY;
        //        startY = endY;
        //        endY = temp;
        //    }

        //    int deltaX = endX - startX;
        //    int deltaY = Math.Abs(endY - startY);

        //    int error = deltaX / 2;

        //    int yStep = (startY < endY) ? 1 : -1;
        //    int y = startY;
        //    for (int x = startX; x <= endX; x++)
        //    {
        //        yield return new Point((isSteep ? y : x), (isSteep ? x : y));
        //        error = error - deltaY;
        //        if (error < 0)
        //        {
        //            y += yStep;
        //            error += deltaX;
        //        }
        //    }
        //}

        public static IEnumerable<Point> GetPointsOnLine(int startX, int startY, int endX, int endY)
        {
            /*
                real deltax := x1 - x0
                real deltay := y1 - y0
                real error := 0
                real deltaerr := abs (deltay / deltax)    // Assume deltax != 0 (line is not vertical),
                      // note that this division needs to be done in a way that preserves the fractional part
                int y := y0
                for x from x0 to x1
                    plot(x,y)
                    error := error + deltaerr
                    while error ≥ 0.5 then
                        plot(x, y)
                        y := y + sign(y1 - y0)
                        error := error - 1.0
            */

            int deltaX = endX - startX;
            int deltaY = endY - startY;

            int deltaError = (deltaX == 0) ? 0 : Math.Abs(deltaY/deltaX);

            int xStep = (deltaX > 0) ? 1 : -1;
            int yStep = (deltaY > 0) ? 1 : -1;

            int y = startY;
            for (int x = startX; x != endX; x += xStep)
            {
                yield return new Point(x, y);
                int error = deltaError;
                while (error >= 0)
                {
                    yield return new Point(x, y);
                    y += yStep;
                    error--;
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
