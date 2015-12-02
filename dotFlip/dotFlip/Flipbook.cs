﻿using System.Collections.Generic;
using System.Windows.Media;
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;
using System.Threading.Tasks;

namespace dotFlip
{
    public delegate void PageChangedHandler(Page currentPage, Page ghostPage);

    public class Flipbook
    {
        private IList<Page> _pages;

        private Page _currentPage;
        private SolidColorBrush _background;
        private Dictionary<string, ITool> _tools;

        private bool _shouldShowGhostStrokes;

        public bool ShowGhostStrokes
        {
            get { return _shouldShowGhostStrokes; }
            set
            {
                _shouldShowGhostStrokes = value;
                RefreshPage();
            }
        }

        public bool IsPlaying { get; set; }

        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                PageChanged(_currentPage, GetPreviousPage(_currentPage)); // Invoke event
            }
        }

        public int PageCount => _pages.Count;

        public Brush Brush => _background;

        public Color BackgroundColor
        {
            get { return _background.Color; }
            set { _background.Color = value; }
        }

        public ITool CurrentTool { get; set; }

        public event PageChangedHandler PageChanged = delegate { };

        public Flipbook(Color backgroundColor)
        {
            _background = new SolidColorBrush();
            BackgroundColor = backgroundColor;
            _tools = new Dictionary<string, ITool>
            {
                {"Pencil", new Pencil()},
                {"Pen", new Pen()},
                {"Highlighter", new Highlighter()},
                {"Eraser", new Eraser(ref _background)},
            };
            CurrentTool = _tools["Pen"];
            CurrentPage = new Page(this);
            _pages = new List<Page> {CurrentPage};
        }

        public void UseTool(string toolToUse)
        {
            if (_tools.ContainsKey(toolToUse))
            {
                CurrentTool = _tools[toolToUse];
            }
        }

        public void DeletePage(Page page)
        {
            int pageIndex = _pages.IndexOf(page);
            if (page == CurrentPage)
            {
                if (PageCount == 1)
                {
                    CurrentPage = new Page(this);
                    _pages = new List<Page> { CurrentPage };
                }
                else
                {
                    MoveToPage(pageIndex - 1);
                }
            }
            _pages.Remove(page);
            RefreshPage();
        }

        public void DeleteAllPages()
        {
            CurrentPage = new Page(this);
            _pages = new List<Page> { CurrentPage };
        }

        public void RefreshPage()
        {
            PageChanged(_currentPage, GetPreviousPage(_currentPage));
        }

        public void MoveToPage(int index)
        {
            if (index < 0)
                index = _pages.Count - 1;

            if (_pages.Count - 1 <= index)
            {
                int pagesToAdd = index - (_pages.Count - 1);
                for (int ii = 0; ii < pagesToAdd; ii++)
                {
                    _pages.Add(new Page(this));
                }
            }

            CurrentPage = _pages[index];
        }

        public void NextPage()
        {
            int currentIndex = _pages.IndexOf(CurrentPage);
            MoveToPage(currentIndex + 1);
        }

        public void PreviousPage()
        {
            int currentIndex = _pages.IndexOf(CurrentPage);
            MoveToPage(currentIndex - 1);
        }

        public int GetPageNumber(Page page)
        {
            return _pages.IndexOf(page) + 1;
        }

        public void CopyPreviousPage()
        {
            int index = _pages.IndexOf(_currentPage);
            if (index > 0)
            {
                Page prevPage = _pages[index - 1];
                _currentPage.CopyPage(prevPage);
            }
        }

        public async void PlayAnimation(int delay)
        {
            do
            {
                foreach (Page page in _pages)
                {
                    await Task.Delay(delay);
                    CurrentPage = page;
                    if (!IsPlaying) break;
                }
            } while (IsPlaying);
        }

        private Page GetPreviousPage(Page p)
        {
            Page prev = null;
            if (_pages != null)
            {
                int index = _pages.IndexOf(p);
                if (index > 0)
                {
                    prev = _pages[index - 1];
                }
            }
            return prev;
        }
    }
}
