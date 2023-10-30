
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sidecab
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        //----------------------------------------------------------------------
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(
                this, new PropertyChangedEventArgs(propertyName));
        }

        //----------------------------------------------------------------------
        protected void RaiseAllPropertiesChanged()
        {
            if (PropertyChanged is not null)
            {
                foreach (var property in GetType().GetProperties())
                {
                    PropertyChanged.Invoke(
                        this, new PropertyChangedEventArgs(property.Name));
                }
            }
        }
    }
}
