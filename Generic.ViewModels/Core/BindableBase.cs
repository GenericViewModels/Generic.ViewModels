using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GenericViewModels.Core
{
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

#pragma warning disable CA1030 // Use events where appropriate
        protected virtual void RaisePropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
#pragma warning restore CA1030 // Use events where appropriate

        public bool SetProperty<T>(ref T item, T value, [CallerMemberName] string propertyName = default!)
        {
            if (EqualityComparer<T>.Default.Equals(item, value)) return false;

            item = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
