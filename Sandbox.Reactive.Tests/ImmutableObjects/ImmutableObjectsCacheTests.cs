using DynamicData;
using FluentAssertions;
using ReactiveUI;
using System.Reactive.Disposables;

namespace Sandbox.Reactive.Tests.ImmutableObjects
{
    internal sealed class ImmutableMessage
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Description { get; }

        public ImmutableMessage(Guid id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
        }

        public ImmutableMessage Set(string? name = null, string? description = null)
        {
            return new ImmutableMessage(
                Id,
                name ?? Name,
                description ?? Description);
        }
    }

    internal sealed class ImmutableMessageViewModel
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

    internal sealed class MutableMessageViewModel : ReactiveObject, IDisposable
    {
        private readonly CompositeDisposable disposables = new CompositeDisposable();
        private readonly ISourceCache<ImmutableMessage, Guid> sourceCache;
        private ImmutableMessage message;

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

        public MutableMessageViewModel(
            ImmutableMessage message,
            ISourceCache<ImmutableMessage, Guid> sourceCache)
        {
            this.message = message;

            sourceCache
                .WatchValue(message.Id)
                .Subscribe(UpdateAll)
                .DisposeWith(disposables);

            this.sourceCache = sourceCache;
        }

        public void Dispose()
        {
            disposables.Dispose();
        }

        private void UpdateAll(ImmutableMessage newMessage)
        {
            message = newMessage;
            this.RaisePropertyChanged(nameof(Name));
            this.RaisePropertyChanged(nameof(Description));
        }
    }

    internal sealed class ImmutableObjectsCacheWithImmutableViewModelsTests
    {
        private const string INITIAL_NAME = "initial name";
        private const string INITIAL_DESCRIPTION = "initial description";
        private const string NEW_NAME = "new name";

        private readonly Guid guid = Guid.NewGuid();

        private readonly SourceCache<ImmutableMessage, Guid> sourceCache
            = new SourceCache<ImmutableMessage, Guid>(message => message.Id);

        [Test]
        public void When_message_is_added_first_and_collection_is_created_later()
        {
            sourceCache.AddOrUpdate(new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION));

            sourceCache
                .Connect()
                .Transform(message => new ImmutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            collection.Should().HaveCount(1);
            collection.Single().Name.Should().Be(INITIAL_NAME);
            collection.Single().Description.Should().Be(INITIAL_DESCRIPTION);
        }

        [Test]
        public void When_collection_is_created_first_and_message_is_added_later()
        {
            sourceCache
                .Connect()
                .Transform(message => new ImmutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            sourceCache.AddOrUpdate(new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION));

            collection.Should().HaveCount(1);
            collection.Single().Name.Should().Be(INITIAL_NAME);
            collection.Single().Description.Should().Be(INITIAL_DESCRIPTION);
        }

        [Test]
        public void When_ViewModel_name_is_changed()
        {
            sourceCache.AddOrUpdate(new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION));

            sourceCache
                .Connect()
                .Transform(message => new ImmutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            collection.Single().Name = NEW_NAME;

            collection.Should().HaveCount(1);
            collection.Single().Name.Should().Be(NEW_NAME);
            sourceCache.Items.Single().Name.Should().Be(NEW_NAME);
        }

        [Test]
        public void When_message_name_is_changed()
        {
            var message = new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION);

            sourceCache.AddOrUpdate(message);

            sourceCache
                .Connect()
                .Transform(message => new ImmutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            sourceCache.AddOrUpdate(message.Set(name:NEW_NAME ));

            collection.Should().HaveCount(1);
            collection.Single().Name.Should().Be(NEW_NAME);
            sourceCache.Items.Single().Name.Should().Be(NEW_NAME);
        }
    }

    internal sealed class ImmutableObjectsCacheWithMutableViewModelTests
    {
        private const string INITIAL_NAME = "initial name";
        private const string INITIAL_DESCRIPTION = "initial description";
        private const string NEW_NAME = "new name";

        private readonly Guid guid = Guid.NewGuid();

        private readonly SourceCache<ImmutableMessage, Guid> sourceCache
            = new SourceCache<ImmutableMessage, Guid>(message => message.Id);

        [Test]
        public void When_message_is_added_first_and_collection_is_created_later()
        {
            sourceCache.AddOrUpdate(new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION));

            sourceCache
                .Connect()
                .Transform(message => new MutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            collection.Should().HaveCount(1);
            collection.Single().Name.Should().Be(INITIAL_NAME);
            collection.Single().Description.Should().Be(INITIAL_DESCRIPTION);
        }

        [Test]
        public void When_collection_is_created_first_and_message_is_added_later()
        {
            sourceCache
                .Connect()
                .Transform(message => new MutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            sourceCache.AddOrUpdate(new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION));

            collection.Should().HaveCount(1);
            collection.Single().Name.Should().Be(INITIAL_NAME);
            collection.Single().Description.Should().Be(INITIAL_DESCRIPTION);
        }

        [Test]
        public void When_ViewModel_name_is_changed()
        {
            sourceCache.AddOrUpdate(new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION));

            sourceCache
                .Connect()
                .Transform(message => new MutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            var viewModel = collection.Single();
            
            viewModel.Name = NEW_NAME;

            collection.Should().HaveCount(1);
            viewModel.Name.Should().Be(NEW_NAME);
            sourceCache.Items.Single().Name.Should().Be(NEW_NAME);
        }

        [Test]
        public void When_message_name_is_changed()
        {
            var message = new ImmutableMessage(guid, INITIAL_NAME, INITIAL_DESCRIPTION);

            sourceCache.AddOrUpdate(message);

            sourceCache
                .Connect()
                .Transform(message => new MutableMessageViewModel(message, sourceCache))
                .Bind(out var collection)
                .Subscribe();

            var viewModel = collection.Single();

            sourceCache.AddOrUpdate(message.Set(name:NEW_NAME ));

            collection.Should().HaveCount(1);
            viewModel.Name.Should().Be(NEW_NAME);
            sourceCache.Items.Single().Name.Should().Be(NEW_NAME);
        }
    }
}
