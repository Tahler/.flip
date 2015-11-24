using System.Collections.Generic;
using System.Windows.Media;
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;
using System;
using System.Collections;
using System.Windows;

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
                PageChanged(currentPage); // Invoke event
            }
        }

        public event PageChangedHandler PageChanged = delegate { };

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
                {"Eraser", new Eraser(ref background)},
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

        public void MoveToPage(int index)
        {
            if (index < 0)
                index = pages.Count - 1;

            if (pages.Count - 1 <= index)
            { 
                int pagesToAdd = index - (pages.Count - 1);
                for (int ii = 0; ii < pagesToAdd; ii++)
                {
                    pages.Add(new Page(this));
                }
                CurrentPage = pages[pages.Count - 1];
            }

            CurrentPage = pages[index];
            if(CurrentPage.ShowGhost) UpdateGhostStrokes();
        }

        public void NextPage()
        {
            int currentIndex = pages.IndexOf(CurrentPage);
            MoveToPage(currentIndex + 1);
        }

        public void PreviousPage()
        {
            int currentIndex = pages.IndexOf(CurrentPage);
            MoveToPage(currentIndex - 1);
        }

        public int GetPageCount()
        {
            return pages.Count;
        }

        public int GetPageNumber(Page page)
        {
            return pages.IndexOf(page) + 1;
        }

        public void CopyPrevPage()
        {
            int index = pages.IndexOf(currentPage);
            if (index > 0)
            {
                Page prevPage = pages[index - 1];
                currentPage.CopyPage(prevPage);
            }
        }


        public void ToggleGhostStrokes()
        {
            currentPage.ShowGhost = !currentPage.ShowGhost;
            int index = pages.IndexOf(currentPage);
            if (currentPage.ShowGhost && index != 0)
            {
                UpdateGhostStrokes();
            }
            else
            {
                //Psuedo refresh the page
                MoveToPage(index - 1);
                MoveToPage(index);
            }
        }
        private void UpdateGhostStrokes()
        {
            int index = pages.IndexOf(CurrentPage);
            if (index > 0)
            {
                Page prevPage = pages[index - 1];
                CurrentPage.UpdateGhostStrokes(prevPage);
            }
        }
    }
}
