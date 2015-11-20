using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace dotFlip
{
    public class Page
    {
        private IList<Visual> visuals;

        public int VisualCount { get; set; }

        public Page()
        {
            visuals = new List<Visual>();
        }

        public Visual this[int index] => visuals[index];

        public void DrawAt(Point point) // will probably need to pass the tool
        {
            DrawingVisual path = new DrawingVisual();
            using (var context = path.RenderOpen())
            {
                context.DrawEllipse(Brushes.Black, null, point, 20, 20);
            }
            visuals.Add(path);
        }
    }
}
