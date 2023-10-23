
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sidecab.Utility
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        //======================================================================
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //======================================================================
        protected void RaiseAllPropertiesChanged()
        {
            if (this.PropertyChanged is object)
            {
                foreach (var p in GetType().GetProperties())
                {
                    this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(p.Name));
                }
            }
        }
    }
}
