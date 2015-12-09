using System.Collections.Generic;
using System.Drawing.Printing;
using System.Windows.Media;
using dotFlip.Tools;
using Pen = dotFlip.Tools.Pen;
using System.Threading.Tasks;
using System;

namespace dotFlip
{
    public delegate void PageChangedHandler(Page currentPage, Page ghostPage);

    public class Flipbook
    {
        private IList<Page> _pages;
        private Page _currentPage;
        private SolidColorBrush _background;
        private Dictionary<string, ITool> _tools;

        private bool _isShowingGhostStrokes;

        public Color[] ColorHistory;

        public bool IsShowingGhostStrokes
        {
            get { return _isShowingGhostStrokes; }
            set
            {
                _isShowingGhostStrokes = value;
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
                RefreshPage();
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
            ColorHistory = new Color[]
            {
                Colors.Black, Colors.White, Colors.Gray,
                Colors.Blue, Colors.Green, Colors.Red,
                Colors.Pink, Colors.Orange, Colors.Orchid
            };
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

        public void Save()
        {
            // Save all drawings
            if (Directory.Exists(SavePath))
            {
                DirectoryInfo savePath = new DirectoryInfo(SavePath);
                foreach (FileInfo file in savePath.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in savePath.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(SavePath, true);
            }
            for (int i = 0; i < _pages.Count; i++)
            {
                var page = _pages[i];
                string pageSavePath = SavePath + @"\Page" + i;
                if (Directory.Exists(pageSavePath))
                {
                    Directory.Delete(pageSavePath, true);
                }
                Directory.CreateDirectory(pageSavePath);

                for (int j = 0; j < page.Drawings.Count; j++)
                {
                    var visual = page.Drawings[j];

                    string drawingSavePath = pageSavePath + @"\Drawing" + j + ".xaml";
                    using (var stream = File.Create(drawingSavePath))
                    {
                        XamlWriter.Save(visual.Drawing, stream);
                    }
                }
            }
            // save colors eventually too
        }

        public void Load()
        {
            if (Directory.Exists(SavePath))
            {
                var flipbookDir = new DirectoryInfo(SavePath);
                List<Page> pages = new List<Page>();
                foreach (var pageDir in flipbookDir.GetDirectories())
                {
                    var page = new Page(this);
                    foreach (var drawingFile in pageDir.GetFiles())
                    {
                        using (var stream = File.Open(drawingFile.FullName, FileMode.Open))
                        {
                            DrawingVisual visual = new DrawingVisual();
                            using (var context = visual.RenderOpen())
                            {
                                context.DrawDrawing((Drawing)XamlReader.Load(stream));
                            }
                            page.Drawings.Add(visual);
                        }
                    }
                    pages.Add(page);
                }
                // use new pages now
                _pages = pages;
                CurrentPage = _pages[0];
            }
        }

        public void UseTool(string toolToUse)
        {
            if (_tools.ContainsKey(toolToUse))
            {
                CurrentTool = _tools[toolToUse];
            }
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
            _pages = new List<Page> { new Page(this) };
            CurrentPage = _pages[0];
        }

        public void RefreshPage()
        {
            PageChanged(_currentPage, _isShowingGhostStrokes ? GetPreviousPage(_currentPage) : null);
        }

        public Page GetPageAt(int index)
        {
            return _pages[index];
        }

        public void MoveToPage(int index)
        {
            if (index < 0) return;

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

        public void CopyPreviousPageToCurrentPage()
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

        internal int UpdateColorHistory(Color c)
        {
            bool alreadyHasColor = false;
            int colorIndex = 0;
            for(int index = 0; index < ColorHistory.Length; index++)
            {
                if (ColorHistory[index].Equals(c))
                {
                    alreadyHasColor = true;
                    colorIndex = index;
                    break;
                }
            }
            if (!alreadyHasColor)
            {
                for (int index = 7; index > 0; index--)
                {
                    ColorHistory[index] = ColorHistory[index - 1];
                }
                ColorHistory[0] = c;
            }
            return colorIndex;
        }
    }
}
