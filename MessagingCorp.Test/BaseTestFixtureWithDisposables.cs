namespace MessagingCorp.Test
{
    public class BaseTestFixtureWithDisposables
    {
        // Field to hold disposables
        protected readonly List<IDisposable> disposables = new List<IDisposable>();

        [SetUp]
        public void Setup()
        {

        }

        [OneTimeTearDown]
        public void TearDown()
        {
            // Dispose of resources
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }
        }
    }
}