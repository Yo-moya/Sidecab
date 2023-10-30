
using System.Runtime.InteropServices;

namespace Sidecab.View
{
    static class SystemAttributes
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern uint GetDoubleClickTime();
    }
}
