
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    //==========================================================================
    public class Selector<T> : Base where T : class
    {
        public ObservableCollection<T> List { get; private set; }

        //----------------------------------------------------------------------
        public T Current
        {
            get { return _current; }
            set
            {
                _current = value;
                RaisePropertyChanged();
            }
        }

        //----------------------------------------------------------------------
        public int Index
        {
            get { return List.IndexOf(_current); }
            set
            {
                int index = Math.Min(Math.Max(0, value), List.Count - 1);
                _current = List[index];
            }
        }


        //======================================================================
        public Selector(IEnumerable<T> source)
        {
            List = new ObservableCollection<T>(source);
            _current = List[0];
        }


        private T _current;
    }
}
