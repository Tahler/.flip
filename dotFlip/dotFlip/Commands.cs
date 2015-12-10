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
        public static ICommand PreviousPage { get; } = new RoutedCommand();
        public static ICommand NextPage { get; } = new RoutedCommand();
        public static ICommand Play { get; } = new RoutedCommand();
        public static ICommand CopyPreviousPage { get; } = new RoutedCommand();
        public static ICommand ToggleGhostStrokes { get; } = new RoutedCommand();
        public static ICommand ClearPage { get; } = new RoutedCommand();
        public static ICommand DeletePage { get; } = new RoutedCommand();
        public static ICommand Restart { get; } = new RoutedCommand();
        public static ICommand Export { get; } = new RoutedCommand();
    }
}
