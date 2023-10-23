
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    //==========================================================================
    public class Selector<T> : Utility.ObservableObject where T : class
    {
        public ObservableCollection<T> List { get; private set; }

        //----------------------------------------------------------------------
        public T Current
        {
            get { return this.selectedObject; }
            set
            {
                this.selectedObject = value;
                RaisePropertyChanged(nameof(this.Current));
            }
        }

        //----------------------------------------------------------------------
        public int Index
        {
            get { return List.IndexOf(this.selectedObject); }
            set
            {
                if ((value < 0) || (value >= this.List.Count))
                {
                    this.Current = null;
                    return;
                }

                this.Current = this.List[value];
            }
        }


        //======================================================================
        public Selector()
        {
            this.List = new ObservableCollection<T>();
        }

        //======================================================================
        public Selector(IEnumerable<T> source)
        {
            SetList(source);
            this.selectedObject = ((this.List.Count > 0) ? this.List[0] : null);
        }

        //======================================================================
        public void SetList(IEnumerable<T> source)
        {
            this.List = new ObservableCollection<T>(source);
        }


        private T selectedObject = null;
    }
}
