
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Media;
using Newtonsoft.Json;

namespace Sidecab.Model
{
    public class Settings
    {
        //----------------------------------------------------------------------
        public int KnobWidth
        {
            get { return  this.data.KnobWidth; }
            set { this.data.KnobWidth = value; }
        }

        //----------------------------------------------------------------------
        public int TreeWidth
        {
            get { return  this.data.TreeWidth; }
            set { this.data.TreeWidth = value; }
        }

        //----------------------------------------------------------------------
        public int TreeFontSize
        {
            get { return  this.data.TreeFontSize; }
            set { this.data.TreeFontSize = value; }
        }

        //----------------------------------------------------------------------
        public int TreeFontSizeLarge
        {
            get { return  this.data.TreeFontSizeLarge; }
            set { this.data.TreeFontSizeLarge = value; }
        }

        //----------------------------------------------------------------------
        public Type.DockPosition DockPosition
        {
            get { return  this.data.DockPosition; }
            set { this.data.DockPosition = value; }
        }

        //----------------------------------------------------------------------
        public Color KnobColor
        {
            get { return  this.data.KnobColor; }
            set { this.data.KnobColor = value; }
        }

        //----------------------------------------------------------------------
        public int DisplayIndex
        {
            get { return this.data.DisplayIndex; }
            set
            {
                using (var monitors = WpfAppBar.MonitorInfo.GetAllMonitors().GetEnumerator())
                {
                    int count = 0;
                    while (monitors.MoveNext()) { count++; }
                    this.data.DisplayIndex = Math.Max(0, Math.Min(count - 1, value));
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
        public Task Load()
        {
            return Task.Run(() =>
            {
                //--------------------------------------------------------------
                try
                {
                    using (var file = new StreamReader(Settings.SaveFilePath))
                    {
                        var json = new JsonSerializer();
                        this.data = json.Deserialize<Data>(new JsonTextReader(file));
                    }
                }
                //--------------------------------------------------------------
                catch
                {
                }
                //--------------------------------------------------------------
            });
        }

        //======================================================================
        public Task Save()
        {
            return Task.Run(() =>
            {
                //--------------------------------------------------------------
                try
                {
                    using (var file = new StreamWriter(Settings.SaveFilePath))
                    {
                        var str = JsonConvert.SerializeObject(this, Formatting.Indented);
                        file.Write(str);
                    }
                }
                //--------------------------------------------------------------
                catch
                {
                }
                //--------------------------------------------------------------
            });
        }


        private Data data = new Data();


        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private class Data
        {
            public int KnobWidth { get; set; } =  10;
            public int TreeWidth { get; set; } = 300;

            public int TreeFontSize { get; set; } = 14;
            public int TreeFontSizeLarge { get; set; } = 20;

            public int DisplayIndex { get; set; } = 0;
            public Type.DockPosition DockPosition { get; set; } = Type.DockPosition.Left;
            public Color KnobColor { get; set; } = Colors.Orange;
        }
    }
}
