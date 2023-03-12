using ReactiveUI;
using Sandbox.Reactive.Model;
using System.ComponentModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace Sandbox.Reactive.ViewModel
{
    public sealed class ReactiveMessageViewModel : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable _disposables = new CompositeDisposable();

        private readonly ReactiveMessage message;

        public string Header
        {
            get => message.Header;
            set => message.Header = value;
        }

        public string Text
        {
            get => message.Text;
            set => message.Text = value;
        }

        public ReactiveMessageViewModel(ReactiveMessage message)
        {
            this.message = message;

            Observable
                .FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
                    handler => message.PropertyChanged += handler,
                    handler => message.PropertyChanged -= handler)
                .Select(pattern => pattern.EventArgs.PropertyName)
                .Subscribe(propertyName => this.RaisePropertyChanged(propertyName))
                .DisposeWith(_disposables);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}
