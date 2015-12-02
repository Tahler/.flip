using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace dotFlip
{
    public static class Commands
    {
        public static RoutedCommand PreviousPage { get; } = new RoutedCommand();
        public static RoutedCommand NextPage { get; } = new RoutedCommand();
        public static RoutedCommand ShowGhostStrokes { get; } = new RoutedCommand();
    }
}
