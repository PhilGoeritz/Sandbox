using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Sandbox.Reactive.Model
{
    public sealed class ReactiveMessage : ReactiveObject
    {
        [Reactive]
        public string Header { get; set; }

        [Reactive]
        public string Text { get; set; }

        public ReactiveMessage(
            string header,
            string text)
        {
            Header = header;
            Text = text;
        }
    }
}
