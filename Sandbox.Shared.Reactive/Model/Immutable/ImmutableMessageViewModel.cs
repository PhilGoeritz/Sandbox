using DynamicData;

namespace Sandbox.Shared.Reactive.Model.Immutable
{
    public sealed class ImmutableMessageViewModel
    {
        private readonly ImmutableMessage message;
        private readonly ISourceCache<ImmutableMessage, Guid> sourceCache;

        public string Name
        {
            get => message.Name;
            set => sourceCache.AddOrUpdate(message.Set(name: value));
        }

        public string Description
        {
            get => message.Description;
            set => sourceCache.AddOrUpdate(message.Set(description: value));
        }

        public ImmutableMessageViewModel(
            ImmutableMessage message,
            ISourceCache<ImmutableMessage, Guid> sourceCache)
        {
            this.message = message;
            this.sourceCache = sourceCache;
        }
    }
}
