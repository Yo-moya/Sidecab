
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sidecab.Presenter
{
    public class Base : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        //======================================================================
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
