
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;

namespace Sidecab.View
{
    public static class ScreenInfo
    {
        //----------------------------------------------------------------------
        public static Rect GetWorkingArea(Window window)
        {
            var pixelsPerDip = VisualTreeHelper.GetDpi(window).PixelsPerDip;
            var screen = Screen.FromPoint(new((int)window.Left, (int)window.Top));

            return new(
                screen.WorkingArea.Left   / pixelsPerDip,
                screen.WorkingArea.Top    / pixelsPerDip,
                screen.WorkingArea.Width  / pixelsPerDip,
                screen.WorkingArea.Height / pixelsPerDip);
        }
    }
}
