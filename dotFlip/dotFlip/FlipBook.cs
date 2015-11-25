using System.Collections.Generic;
using System.Windows.Media;
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;
using System.Threading.Tasks;

namespace dotFlip
{
    public delegate void PageChangedHandler(Page currentPage);

    public class Flipbook
    {
        private IList<Page> _pages;

        private Page _currentPage;
        private SolidColorBrush _background;
        private Dictionary<string, ITool> _tools;

        public Page CurrentPage
        {
            get { return _currentPage; }
            set
            {
                _currentPage = value;
                PageChanged(_currentPage); // Invoke event
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
        }
        public void RefreshPage()
        {
            Page page = new Page(this);
            _pages.Add(page);
            int currentIndex = _pages.IndexOf(CurrentPage);
            MoveToPage(_pages.IndexOf(page));
            MoveToPage(currentIndex);
            DeletePage(page);
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
            if (CurrentPage.ShowGhostStrokes) UpdateGhostStrokes();
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

        public void CopyPrevPage()
        {
            int index = _pages.IndexOf(_currentPage);
            if (index > 0)
            {
                Page prevPage = _pages[index - 1];
                _currentPage.CopyPage(prevPage);
            }
        }

        public void ToggleGhostStrokes()
        {
            _currentPage.ShowGhostStrokes = !_currentPage.ShowGhostStrokes;
            int index = _pages.IndexOf(_currentPage);
            if (_currentPage.ShowGhostStrokes && index != 0)
            {
                UpdateGhostStrokes();
            }
            else
            {
                // Pseudo refresh the page
                MoveToPage(index - 1);
                MoveToPage(index);
            }
        }

        private void UpdateGhostStrokes()
        {
            int index = _pages.IndexOf(CurrentPage);
            if (index > 0)
            {
                Page prevPage = _pages[index - 1];
                CurrentPage.UpdateGhostStrokes(prevPage);
            }

        }

        public async void PlayAnimation(int value)
        {
            foreach (Page page in _pages)
            {
                await Task.Delay(value);
                CurrentPage = page;
            }
        }
    }
}
