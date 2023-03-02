using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sandbox.Shared
{
    public class ObjectBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetAndRaiseEvent<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;
            RaisePropertyChanged(propertyName);
        }

        protected void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged is not null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}