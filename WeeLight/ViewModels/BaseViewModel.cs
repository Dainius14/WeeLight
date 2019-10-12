using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WeeLight.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void Set<T>(T value, Action<T> doAction, [CallerMemberName] string propertyName = null)
        {
            doAction(value);
            OnPropertyChanged(propertyName);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
