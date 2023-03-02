using FluentAssertions;
using Sandbox.Reactive.Model;
using Sandbox.Reactive.ViewModel;
using Sandbox.Tests.Shared;

namespace Sandbox.Reactive.Tests.ReactiveMessageViewModelTests
{
    public abstract class ReactiveMessageViewModelTests : TestBase
    {
        protected const string NEW_HEADER = "new header";
        protected const string NEW_TEXT = "new text";

        protected readonly ReactiveMessage message;
        protected readonly ReactiveMessageViewModel messageViewModel;

        protected ReactiveMessageViewModelTests()
        {
            message = new ReactiveMessage("header", "text");
            messageViewModel = new ReactiveMessageViewModel(message);
        }
    }

    public sealed class WhenMessagePropertyIsChanged : ReactiveMessageViewModelTests
    {
        protected override void BecauseOf()
        {
            message.Header = NEW_HEADER;
            message.Text = NEW_TEXT;
        }

        [Test]
        public void Then_ViewModel_Header_should_be_NEW_HEADER()
        {
            messageViewModel.Header.Should().Be(NEW_HEADER);
        }

        [Test]
        public void Then_ViewModel_Text_should_be_NEW_TEXT()
        {
            messageViewModel.Text.Should().Be(NEW_TEXT);
        }
    }

    public sealed class WhenMessageViewModelPropertyIsChanged : ReactiveMessageViewModelTests
    {
        protected override void BecauseOf()
        {
            messageViewModel.Header = NEW_HEADER;
            messageViewModel.Text = NEW_TEXT;
        }

        [Test]
        public void Then_Message_Header_should_be_NEW_HEADER()
        {
            message.Header.Should().Be(NEW_HEADER);
        }

        [Test]
        public void Then_Message_Text_should_be_NEW_TEXT()
        {
            message.Text.Should().Be(NEW_TEXT);
        }
    }
}
