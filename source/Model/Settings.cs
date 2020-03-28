using System.Windows.Media;

namespace Sidecab.Model
{
    public class Settings
    {
        public int KnobWidth { get; set; } = 10;
        public int TreeWidth { get; set; } = 300;

        public Color KnobColor { get; set; } = Colors.Gray;
        public Color TreeFontColor { get; set; } = Colors.Black;
        public Color TreeBackgroundColor { get; set; } = Colors.White;
    }
}
