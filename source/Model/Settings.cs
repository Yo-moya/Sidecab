
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
            get { return this.displayIndex; }
            set
            {
                using (var monitors = WpfAppBar.MonitorInfo.GetAllMonitors().GetEnumerator())
                {
                    int monitorCount = 0;
                    while (monitors.MoveNext()) { monitorCount++; }

                    if (this.displayIndex >= monitorCount)
                    {
                        this.displayIndex = monitorCount - 1;
                    }
                }
            }
        }

        //----------------------------------------------------------------------
        private static string SaveFilePath
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
                using (var file = new StreamReader(Settings.SaveFilePath))
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
                using (var file = new StreamWriter(Settings.SaveFilePath))
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


        private int displayIndex = 0;
    }

    //==========================================================================
    public enum DockPosition
    {
        Left ,
        Right,
    }
}
