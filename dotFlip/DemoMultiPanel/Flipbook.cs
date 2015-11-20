using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DemoMultiPanel
{
    public class Flipbook
    {
        private IList<Page> pages;

        public event PageChangedHandler CurrentPageChanged = delegate { };

        private Page currentPage;

        public Page CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;
                CurrentPageChanged(currentPage);
            }
        }

        public Flipbook()
        {
            CurrentPage = new Page(Brushes.AliceBlue);
            pages = new List<Page> {CurrentPage};
            pages.Add(new Page(Brushes.Yellow));
        }

        public void NextPage()
        {
            int currentIndex = pages.IndexOf(CurrentPage);
            this.CurrentPage = pages[currentIndex + 1];
        }

        public void PreviousPage()
        {
            int currentIndex = pages.IndexOf(CurrentPage);
            this.CurrentPage = pages[currentIndex - 1];
        }
    }

    public delegate void PageChangedHandler(Page currentPage);
}
