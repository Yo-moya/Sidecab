
using System;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Sidecab.Utility
{
    internal class WindowAttributes
    {
        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, GWL nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, GWL nIndex, IntPtr dwNewLong);


        private const long WS_EX_TOPMOST  = 0x00000008L;
        private const long WS_MAXIMIZEBOX = 0x00010000L;
        private const long WS_MINIMIZEBOX = 0x00020000L;

        //----------------------------------------------------------------------
        public enum GWL : int
        {
            GWL_WNDPROC    = ( -4),
            GWL_HINSTANCE  = ( -6),
            GWL_HWNDPARENT = ( -8),
            GWL_STYLE      = (-16),
            GWL_EXSTYLE    = (-20),
            GWL_USERDATA   = (-21),
            GWL_ID         = (-12)
        }


        //======================================================================
        public static void SetAsToolWindow(Window window)
        {
            var helper = new WindowInteropHelper(window);
            var style = GetWindowLongPtr(helper.Handle, GWL.GWL_EXSTYLE);

            style = new IntPtr(style.ToInt64() | WS_EX_TOPMOST);
            SetWindowLongPtr(helper.Handle, GWL.GWL_EXSTYLE, style);
        }

        //======================================================================
        public static void HideMinimizeButton(Window window)
        {
            var helper = new WindowInteropHelper(window);
            var style = GetWindowLongPtr(helper.Handle, GWL.GWL_STYLE);

            style = new IntPtr(style.ToInt64() & ~WS_MINIMIZEBOX);
            SetWindowLongPtr(helper.Handle, GWL.GWL_STYLE, style);
        }
    }
}
