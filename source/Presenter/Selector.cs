
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    //==========================================================================
    public class Selector<T> : Utility.NoticeableVariables where T : class
    {
        public ObservableCollection<T> List { get; private set; }

        //----------------------------------------------------------------------
        public T Current
        {
            get { return this.current; }
            set
            {
                this.current = value;
                RaisePropertyChanged(nameof(this.Current));
            }
        }

        //----------------------------------------------------------------------
        public int Index
        {
            get { return List.IndexOf(this.current); }
            set
            {
                int index = Math.Min(Math.Max(0, value), this.List.Count - 1);
                this.Current = List[index];
            }
        }


        //======================================================================
        public Selector(IEnumerable<T> source)
        {
            this.List = new ObservableCollection<T>(source);
            this.current = this.List[0];
        }


        private T current;
    }
}
