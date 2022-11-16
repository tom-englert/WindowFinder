using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace WindowFinder
{
    /// <summary>
    /// View model for the TopLevelWindowsView. Provide a list of top level windows and some information about the space they occupy.
    /// All coordinates are native window coordinates, it's up to the view to translate this properly.
    /// </summary>
    public class TopLevelWindowsViewModel : DependencyObject
    {
        /// <summary>
        /// Total width needed to display all windows.
        /// </summary>
        public double TotalWidth
        {
            get { return (double)GetValue(TotalWidthProperty); }
            set { SetValue(TotalWidthProperty, value); }
        }
        public static readonly DependencyProperty TotalWidthProperty =
            DependencyProperty.Register("TotalWidth", typeof(double), typeof(TopLevelWindowsViewModel));

        /// <summary>
        /// Total height needed to display all windows.
        /// </summary>
        public double TotalHeight
        {
            get { return (double)GetValue(TotalHeightProperty); }
            set { SetValue(TotalHeightProperty, value); }
        }
        public static readonly DependencyProperty TotalHeightProperty =
            DependencyProperty.Register("TotalHeight", typeof(double), typeof(TopLevelWindowsViewModel));

        /// <summary>
        /// Margin needed to ensure all windows are displayed within the scrollable area.
        /// </summary>
        public Thickness TotalMargin
        {
            get { return (Thickness)GetValue(TotalMarginProperty); }
            set { SetValue(TotalMarginProperty, value); }
        }
        public static readonly DependencyProperty TotalMarginProperty =
            DependencyProperty.Register("TotalMargin", typeof(Thickness), typeof(TopLevelWindowsViewModel));

        /// <summary>
        /// All top level windows to display.
        /// </summary>
        public IEnumerable<WindowItemViewModel> TopLevelWindows
        {
            get { return (IEnumerable<WindowItemViewModel>)GetValue(TopLevelWindowsProperty); }
            set { SetValue(TopLevelWindowsProperty, value); }
        }
        public static readonly DependencyProperty TopLevelWindowsProperty =
            DependencyProperty.Register("TopLevelWindows", typeof(IEnumerable<WindowItemViewModel>), typeof(TopLevelWindowsViewModel));

        /// <summary>
        /// Refresh all properties.
        /// </summary>
        internal void Refresh()
        {
            var topLevelWindows = new List<WindowItemViewModel>();

            NativeMethods.EnumWindows(delegate (IntPtr hwnd, IntPtr lParam)
                {
                    if (NativeMethods.IsWindowVisible(hwnd))
                    {
                        long style = NativeMethods.GetWindowLong(hwnd, NativeMethods.GWL_STYLE);
                        if ((style & (NativeMethods.WS_MAXIMIZE | NativeMethods.WS_MINIMIZE)) == 0)
                        {
                            topLevelWindows.Add(new WindowItemViewModel(hwnd));
                        }
                    }

                    return 1;
                }
                , IntPtr.Zero);

            // Now calculate total bounds of all windows
            var xMin = topLevelWindows.Select(window => window.Rect.Left).Min();
            var xMax = topLevelWindows.Select(window => window.Rect.Right).Max();
            var yMin = topLevelWindows.Select(window => window.Rect.Top).Min();
            var yMax = topLevelWindows.Select(window => window.Rect.Bottom).Max();

            TotalWidth = xMax - xMin;
            TotalHeight = yMax - yMin;
            TotalMargin = new Thickness(-xMin, -yMin, xMin, yMin);
            TopLevelWindows = topLevelWindows;
        }
    }
}
