using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace WindowFinder
{
    /// <summary>
    /// Item view model for one item in the window list.
    /// All coordinates are native window coordinates, it's up to the view to translate this properly.
    /// </summary>
    public class WindowItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Create a new object for the window with the given handle.
        /// </summary>
        /// <param name="handle">The handle of the window.</param>
        public WindowItemViewModel(IntPtr handle)
        {
            this.Handle = handle;
        }

        /// <summary>
        /// The handle of this window.
        /// </summary>
        public IntPtr Handle { get; private set; }

        /// <summary>
        /// The window text of this window.
        /// </summary>
        public string Text
        {
            get
            {
                var stringBuilder = new StringBuilder(256);
                NativeMethods.GetWindowText(Handle, stringBuilder, stringBuilder.Capacity);
                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// The process image file path.
        /// </summary>
        public string Process
        {
            get
            {
                var processIdPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(int)));
                NativeMethods.GetWindowThreadProcessId(Handle, processIdPtr);

                var processId = (int) Marshal.PtrToStructure(processIdPtr, typeof(int));
                var hProcess = NativeMethods.OpenProcess(NativeMethods.PROCESS_ALL_ACCESS, false, processId);
                var stringBuilder = new StringBuilder(256);
                NativeMethods.GetProcessImageFileName(hProcess, stringBuilder, stringBuilder.Capacity);

                NativeMethods.CloseHandle(hProcess);
                Marshal.FreeCoTaskMem(processIdPtr);

                return stringBuilder.ToString();
            }
        }

        /// <summary>
        /// The windows rectangle.
        /// </summary>
        public Rect Rect
        {
            get
            {
                NativeMethods.Rect nativeRect;
                NativeMethods.GetWindowRect(Handle, out nativeRect);
                var windowRect = new Rect(nativeRect.TopLeft(), nativeRect.BottomRight());
                return windowRect;
            }
            set
            {
                NativeMethods.SetWindowPos(Handle, IntPtr.Zero,
                    (int) (value.Left),
                    (int) (value.Top),
                    0, 0, NativeMethods.SWP_NOACTIVATE | NativeMethods.SWP_NOSIZE | NativeMethods.SWP_NOZORDER);

                ReportPropertyChanged("Rect");
            }
        }

        #region INotifyPropertyChanged Members

        private void ReportPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}