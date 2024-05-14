
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Sidecab.Presenter
{
    public class Selector<T> : ObservableObject where T : class
    {
        public ObservableCollection<T> List { get; private set; } = [];

        //----------------------------------------------------------------------
        public T? Current
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                RaisePropertyChanged(nameof(Current));
            }
        }

        //----------------------------------------------------------------------
        public int Index
        {
            get
            {
                return (_selectedObject is null)
                    ? -1 : List.IndexOf(_selectedObject);
            }
            set
            {
                if ((value < 0) || (value >= List.Count))
                {
                    Current = null;
                    return;
                }

                Current = List[value];
            }
        }


        private T? _selectedObject = null;



        //----------------------------------------------------------------------
        public Selector()
        {
        }

        //----------------------------------------------------------------------
        public Selector(IEnumerable<T> source)
        {
            SetList(source);
            _selectedObject = (List.Count > 0) ? List[0] : null;
        }

        //----------------------------------------------------------------------
        public void SetList(IEnumerable<T> source)
        {
            List = new ObservableCollection<T>(source);
        }
    }
}
