using DynamicData;
using Sandbox.Reactive.Model;
using Sandbox.Tests.Shared;

namespace Sandbox.Reactive.Tests.ReactiveMessageSourceListTests
{
    public abstract class ReactiveMessageSourceListTest : TestBase
    {
        public ISourceList<ReactiveMessage> MessageSourceList { get; } = new SourceList<ReactiveMessage>();

        
    }
}
