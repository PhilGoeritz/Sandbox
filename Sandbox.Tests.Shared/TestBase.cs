namespace Sandbox.Tests.Shared
{
    public abstract class TestBase
    {
        protected virtual void EstablishContext() { }
        protected virtual void BecauseOf() { }

        [SetUp]
        public void Setup()
        {
            EstablishContext();
            BecauseOf();
        }
    }
}
