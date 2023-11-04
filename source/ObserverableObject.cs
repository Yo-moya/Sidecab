
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sidecab
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        // Returns true if value-type property modified
        //----------------------------------------------------------------------
        protected bool ModifyProperty<T>(ref T target, T newValue,
            [CallerMemberName] string propertyName = "") where T : struct
        {
            if (EqualityComparer<T>.Default.Equals(target, newValue)) return false;

            target = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }

        //----------------------------------------------------------------------
        protected bool ModifyNestedProperty<T>(
            object propertyOwner, string propertyName, T newValue) where T : struct
        {
            var property = propertyOwner.GetType().GetProperty(propertyName);

            if ((property.GetValue(propertyOwner) is not T oldValue)
                || EqualityComparer<T>.Default.Equals(oldValue, newValue)) return false;

            property.SetValue(propertyOwner, newValue);
            RaisePropertyChanged(propertyName);
            return true;
        }

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
