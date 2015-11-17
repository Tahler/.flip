using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using dotFlip.Tools;

namespace dotFlip
{
    public class Stroke : FrameworkElement
    {
        public Path Path { get; private set; }
        private PathGeometry geometry;
        private PathFigure figure;

        public Stroke(Point startingPoint, Brush brush, double thickness)
        {
            this.figure = new PathFigure {StartPoint = startingPoint};

            this.geometry = new PathGeometry();
            this.geometry.Figures.Add(figure);

            this.Path = new Path
            {
                Stroke = brush,
                StrokeThickness = thickness
            };
        }

        public void ConnectTo(Point point)
        {
            // Create the new PathSegment
            LineSegment segment = new LineSegment(point, true);
            figure.Segments.Add(segment);

            // Replace the PathFigure
            geometry.Figures.RemoveAt(geometry.Figures.Count - 1);
            geometry.Figures.Add(figure);

            // Replace the Path on the canvas
            var stroke = Path.Stroke;
            var thickness = Path.StrokeThickness;

            Path = new Path
            {
                Stroke = Path.Stroke,
                StrokeThickness = Path.StrokeThickness,
                Data = geometry
            };
        }
    }
}