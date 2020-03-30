
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    //==========================================================================
    public class Selector<T> : Base where T : class
    {
        //----------------------------------------------------------------------
        public ObservableCollection<T> List
        {
            get { return _list; }
        }

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
            get { return _list.IndexOf(_current); }
            set
            {
                int index = Math.Min(Math.Max(0, value), _list.Count - 1);
                _current = _list[index];
            }
        }


        //======================================================================
        public Selector(IEnumerable<T> source)
        {
            _list = new ObservableCollection<T>(source);
            _current = _list[0];
        }


        private ObservableCollection<T> _list;
        private T _current;
    }
}
