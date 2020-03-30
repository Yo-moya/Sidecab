
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Sidecab.Model
{
    public class Settings
    {
        public int KnobWidth { get; set; } = 10;
        public int TreeWidth { get; set; } = 300;

        public DockPosition DockPosition { get; set; } = DockPosition.Left;
        public Color KnobColor { get; set; } = Colors.Orange;

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get { return _displayIndex; }
            set
            {
                using (var monitors = WpfAppBar.MonitorInfo.GetAllMonitors().GetEnumerator())
                {
                    int count = 0;
                    while (monitors.MoveNext()) { count++; }
                    if (_displayIndex >= count) { _displayIndex = count - 1; }
                }
            }
        }

        //----------------------------------------------------------------------
        private static string SettingFilePath
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory() + @"\Settings.json";
            }
        }


        //======================================================================
        public static Settings Load()
        {
            //------------------------------------------------------------------
            try
            {
                using (var file = new StreamReader(SettingFilePath))
                {
                    var json = new JsonSerializer();
                    return json.Deserialize<Settings>(new JsonTextReader(file));
                }
            }
            //------------------------------------------------------------------
            catch
            {
            }
            //------------------------------------------------------------------

            return null;
        }

        //======================================================================
        public void Save()
        {
            //------------------------------------------------------------------
            try
            {
                using (var file = new StreamWriter(SettingFilePath))
                {
                    var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                    file.Write(str);
                }
            }
            //------------------------------------------------------------------
            catch
            {
            }
            //------------------------------------------------------------------
        }


        private int _displayIndex = 0;
    }

    //==========================================================================
    public enum DockPosition
    {
        Left ,
        Right,
    }
}
