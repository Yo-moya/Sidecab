
using System;
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Core
    {
        public event Action RootListChanged;

        public Settings   Settings { get; private set; }
        public List<Root> RootList { get; private set; }


        //======================================================================
        public Core()
        {
            this.Settings = Settings.Load() ?? new Settings();
            this.RootList = Root.EnumerateDrives();
        }

        //======================================================================
        public void SetRootDirectory(Directory directory)
        {
            this.RootList.RemoveAll(r => r.IsDrive == false);
            this.RootList.Add(new Root(directory));

            this.RootListChanged?.Invoke();
        }
    }
}
