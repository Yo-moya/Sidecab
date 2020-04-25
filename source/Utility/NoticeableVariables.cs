
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sidecab.Utility
{
    public class NoticeableVariables : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        //======================================================================
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
