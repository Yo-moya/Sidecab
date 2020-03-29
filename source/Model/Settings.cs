
using System.IO;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Sidecab.Model
{
    public class Settings
    {
        public int   KnobWidth { get; set; } = 10;
        public int   TreeWidth { get; set; } = 300;
        public Color KnobColor { get; set; } = Colors.Orange;

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
                    var json = new JsonSerializer();
                    json.Serialize(new JsonTextWriter(file), this);
                }
            }
            //------------------------------------------------------------------
            catch
            {
            }
            //------------------------------------------------------------------
        }
    }
}
