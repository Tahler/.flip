using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotFlip
{
    class FlipBook
    {
        private List<Page> pages;

        public FlipBook()
        {
            pages = new List<Page>();
        }

        public void AddPage(Page p)
        {
            pages.Add(p);
        }

        public void RemovePage(Page p)
        {
            pages.Remove(p);
        }

        public void RemoveAt(int index)
        {
            pages.RemoveAt(index);
        }

        public Page GetPageAt(int index)
        {
            return pages[index];
        }

        public int GetPageNumber(Page p)
        {
            return pages.IndexOf(p);
        }
    }
}
