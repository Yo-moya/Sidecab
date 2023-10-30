
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sidecab.Model
{
    public class Settings
    {
        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get => _data.TreeWidth;
            set => _data.TreeWidth = value;
        }

        //----------------------------------------------------------------------
        public int TreeFontSize
        {
            get => _data.TreeFontSize;
            set => _data.TreeFontSize = value;
        }

        //----------------------------------------------------------------------
        public int TreeFontSizeLarge
        {
            get => _data.TreeFontSizeLarge;
            set => _data.TreeFontSizeLarge = value;
        }

        //----------------------------------------------------------------------
        public Type.DockPosition DockPosition
        {
            get =>_data.DockPosition;
            set =>_data.DockPosition = value;
        }

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get => _data.DisplayIndex;
            set => _data.DisplayIndex = value;
        }

        //----------------------------------------------------------------------
        private static string SaveFilePath =>
            System.IO.Directory.GetCurrentDirectory() + @"\Settings.json";



        //----------------------------------------------------------------------
        public Task Load()
        {
            return Task.Run(() =>
            {
                try
                {
                    using var file = new StreamReader(Settings.SaveFilePath);
                    var json = new JsonSerializer();
                    _data = json.Deserialize<Data>(new JsonTextReader(file));
                }
                catch {}
            });
        }

        //----------------------------------------------------------------------
        public Task Save()
        {
            return Task.Run(() =>
            {
                try
                {
                    using var file = new StreamWriter(Settings.SaveFilePath);
                    var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                    file.Write(str);
                }
                catch {}
            });
        }


        private Data _data = new Data();

        //======================================================================
        private class Data
        {
            public int TreeWidth { get; set; } = 300;

            public int TreeFontSize { get; set; } = 14;
            public int TreeFontSizeLarge { get; set; } = 20;

            public int DisplayIndex { get; set; } = 0;
            public Type.DockPosition DockPosition { get; set; } = Type.DockPosition.Left;
        }
    }
}
