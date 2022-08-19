using System;
using System.ComponentModel;

namespace MicrocontrollerSimulation.ViewModels.Base
{
    public class ViewModelBase : INotifyPropertyChanged, IDisposable
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
