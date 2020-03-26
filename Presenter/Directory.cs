
using System.Collections.Generic;

namespace Sidecab.Presenter
{
    public class Directory : Base
    {
        public string Path { get { return _model.Path; } }
        public string Name { get { return _model.Name; } }

        //----------------------------------------------------------------------
        public List<Directory> Children
        {
            get
            {
                //--------------------------------------------------------------
                var subdirMd = _model.ListSubdirectories();
                if (subdirMd != null)
                {
                    var subdirPr = new List<Directory>(subdirMd.Count);
                    foreach (var dirMd in subdirMd)
                    {
                        subdirPr.Add(new Directory(dirMd));
                    }

                    return subdirPr;
                }
                //--------------------------------------------------------------

                return null;
            }
        }


        //======================================================================
        public Directory(Model.Directory model)
        {
            _model = model;
        }

        //======================================================================
        public void Open()
        {
            _model.Open();
        }


        private Model.Directory _model;
    }
}
