using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;

namespace dotFlip
{
    public delegate void PageChangedHandler(Page currentPage);

    public class Flipbook
    {
        private IList<Page> pages;

        private Page currentPage;
        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                CurrentPageChanged(currentPage); // Invoke event
            }
        }

        public event PageChangedHandler CurrentPageChanged = delegate { };

        private SolidColorBrush background;

        public Brush Brush => background;

        public Color BackgroundColor
        {
            get { return background.Color; }
            set
            {
                background.Color = value;
            }
        }

        private Dictionary<string, ITool> tools;
        private ITool currentTool;

        public ITool CurrentTool
        {
            get { return currentTool; }
            set { currentTool = value; }
        }

        public Flipbook(Color backgroundColor)
        {
            background = new SolidColorBrush();
            BackgroundColor = backgroundColor;

            tools = new Dictionary<string, ITool>
            {
                {"Pencil", new Pencil()},
                {"Pen", new Pen()},
                {"Highlighter", new Highlighter()},
//                {"Eraser", new Eraser(ref background)},
            };
            CurrentTool = tools["Pen"];

            CurrentPage = new Page(this);
            pages = new List<Page> { CurrentPage };

        }

        public void UseTool(string toolToUse)
        {
            if (tools.ContainsKey(toolToUse))
            {
                CurrentTool = tools[toolToUse];
            }
        }

        public void NextPage()
        {
            int currentIndex = pages.IndexOf(CurrentPage);

            if (currentIndex == pages.Count - 1) // If at the end
            {
                pages.Add(new Page(this));
            }
            CurrentPage = pages[currentIndex + 1];
        }

        public void PreviousPage()
        {
            int currentIndex = pages.IndexOf(CurrentPage);
            if (currentIndex > 0)
            {
                CurrentPage = pages[currentIndex - 1];
            }
        }
    }
}
