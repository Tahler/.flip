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
        public static RoutedCommand Play { get; } = new RoutedCommand();
        public static RoutedCommand CopyPreviousPage { get; } = new RoutedCommand();
        public static RoutedCommand ToggleGhostStrokes { get; } = new RoutedCommand();
        public static RoutedCommand ClearPage { get; } = new RoutedCommand();
        public static RoutedCommand DeletePage { get; } = new RoutedCommand();
        public static RoutedCommand Restart { get; } = new RoutedCommand();
        public static RoutedCommand Export { get; } = new RoutedCommand();
    
    }
}
