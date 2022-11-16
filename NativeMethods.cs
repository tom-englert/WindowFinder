using System;
using System.Runtime.InteropServices;
using System.Text;

namespace WindowFinder
{
    internal static class NativeMethods
    {
        public const int GWL_HINSTANCE = -6;
        public const int GWL_STYLE = -16;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_MAXIMIZE = 0x01000000;
        public const int SWP_NOACTIVATE = 16;
        public const int SWP_NOZORDER = 4;
        public const int SWP_NOSIZE = 1;

        public const int PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | (SYNCHRONIZE | 4095));
        public const int STANDARD_RIGHTS_REQUIRED = 983040;
        public const int SYNCHRONIZE = 1048576;

        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        public delegate int WNDENUMPROC(IntPtr hwnd, IntPtr lParam);

        [StructLayoutAttribute(LayoutKind.Sequential)]
        public struct Rect
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImportAttribute("user32.dll", EntryPoint = "EnumWindows")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

        [DllImportAttribute("user32.dll", EntryPoint = "GetWindowRect")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool GetWindowRect([In] IntPtr hWnd, [Out] out Rect lpRect);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "Result is of no use")]
        [DllImportAttribute("user32.dll", EntryPoint = "GetWindowTextW")]
        public static extern void GetWindowText([In] IntPtr hWnd, [Out] [MarshalAsAttribute(UnmanagedType.LPWStr)] StringBuilder lpString, int nMaxCount);

        [DllImportAttribute("user32.dll", EntryPoint = "IsWindowVisible")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible([In] IntPtr hWnd);

        [DllImportAttribute("user32.dll", EntryPoint = "GetWindowLongW")]
        public static extern int GetWindowLong([In] IntPtr hWnd, int nIndex);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "Result is of no use")]
        [DllImportAttribute("user32.dll", EntryPoint = "GetWindowThreadProcessId")]
        public static extern void GetWindowThreadProcessId([In] IntPtr hWnd, IntPtr lpdwProcessId);

        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, [MarshalAsAttribute(UnmanagedType.Bool)] bool bInheritHandle, int dwProcessId);

        [DllImportAttribute("kernel32.dll", EntryPoint = "CloseHandle")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool CloseHandle([In] IntPtr hObject);

        [DllImportAttribute("Psapi.dll", EntryPoint = "GetProcessImageFileNameW", CharSet = CharSet.Unicode)]
        public static extern void GetProcessImageFileName([In] IntPtr hProcess, [Out] [MarshalAsAttribute(UnmanagedType.LPWStr)] StringBuilder lpFilename, int nSize);

        [DllImportAttribute("user32.dll", EntryPoint = "SetWindowPos")]
        [return: MarshalAsAttribute(UnmanagedType.Bool)]
        public static extern bool SetWindowPos([In] IntPtr hWnd, [In] IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    }
}
