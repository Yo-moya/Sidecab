
using System.Collections.Generic;

namespace Sidecab.Model
{
    public class Core
    {
        public Settings Settings { get; private set; }
        public List<Root> RootLocations { get; private set; }
        public Root CurrentRoot { get; private set; }


        //======================================================================
        public Core()
        {
            Settings = Settings.Load() ?? new Settings();
            RootLocations = Root.EnumerateDrives();
            CurrentRoot = (RootLocations.Count > 0) ? RootLocations[0] : null;
        }

        //======================================================================
        public void SetRoot(string path)
        {
            foreach (var root in RootLocations)
            {
                if (root.Path == path)
                {
                    CurrentRoot = root;
                    break;
                }
            }
        }
    }
}
