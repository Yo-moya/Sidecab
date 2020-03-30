
using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Sidecab.Utility
{
    internal class WindowAttributes
    {
        //======================================================================
        public static void SetAsToolWindow(Window window)
        {
            var helper = new WindowInteropHelper(window);

            var style = GetWindowLongPtr(helper.Handle, GWL.EXSTYLE);
            style = new IntPtr(style.ToInt64() | WS_EX_TOPMOST);

            SetWindowLongPtr(helper.Handle, GWL.EXSTYLE, style);
        }

        //======================================================================
        public static void HideTitleBarIcon(Window window)
        {
            var helper = new WindowInteropHelper(window);

            SendMessage(helper.Handle, WM_SETICON, new IntPtr(1), IntPtr.Zero);
            SendMessage(helper.Handle, WM_SETICON,   IntPtr.Zero, IntPtr.Zero);

            var style = GetWindowLongPtr(helper.Handle, GWL.EXSTYLE);
            style = new IntPtr(style.ToInt64() | WS_EX_DLGMODALFRAME);

            SetWindowLongPtr(helper.Handle, GWL.EXSTYLE, style);

            SetWindowPos(helper.Handle, IntPtr.Zero, 0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
        }

        //======================================================================
        public static void HideMinimizeButton(Window window)
        {
            var helper = new WindowInteropHelper(window);

            var style = GetWindowLongPtr(helper.Handle, GWL.STYLE);
            style = new IntPtr(style.ToInt64() & ~WS_MINIMIZEBOX);

            SetWindowLongPtr(helper.Handle, GWL.STYLE, style);
        }


        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
            int x, int y, int width, int height, uint flags);


        private const uint WM_SETICON = 0x0080;

        private const long WS_EX_DLGMODALFRAME = 0x00000001L;
        private const long WS_EX_TOPMOST       = 0x00000008L;
        private const long WS_MAXIMIZEBOX      = 0x00010000L;
        private const long WS_MINIMIZEBOX      = 0x00020000L;

        private const uint SWP_NOMOVE       = 0x0002;
        private const uint SWP_NOSIZE       = 0x0001;
        private const uint SWP_NOZORDER     = 0x0004;
        private const uint SWP_FRAMECHANGED = 0x0020;

        //----------------------------------------------------------------------
        private enum GWL : int
        {
            WNDPROC    =  -4,
            HINSTANCE  =  -6,
            HWNDPARENT =  -8,
            STYLE      = -16,
            EXSTYLE    = -20,
            USERDATA   = -21,
            ID         = -12,
        }
    }
}
