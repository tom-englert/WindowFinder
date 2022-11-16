using System;
using System.Windows;
using System.Windows.Interop;

namespace WindowFinder
{
    internal static class CustomExtensions
    {
        public static Point TopLeft(this NativeMethods.Rect rect)
        {
            return new Point(rect.left, rect.top);
        }

        public static Point BottomRight(this NativeMethods.Rect rect)
        {
            return new Point(rect.right, rect.bottom);
        }

        /// <summary>
        /// Get the handle of the window containing the given UIElement.
        /// </summary>
        /// <returns>The window handle.</returns>
        public static IntPtr GetHandle(this UIElement self)
        {
            return ((IWin32Window)PresentationSource.FromDependencyObject(self)).Handle;
        }
    }
}
