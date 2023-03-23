namespace Sandbox.Shared.Reactive.Model.Immutable
{
    public sealed class ImmutableMessage
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
}
