using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MessagingCorp.Providers.API;
using MessagingCorp.Services.API;
using MessagingCorp.Services.Core;
using Moq;
using Ninject;
using Ninject.MockingKernel;
using NUnit.Framework;
namespace MessagingCorp.Test.ServiceTests
{
    [TestFixture]
    [Parallelizable]
    public class MessagingCorpControllerTest : BaseTestFixtureWithDisposables
    {
        /*
        private IKernel kernel;

        private IMessageCorpController messageCorpController;
        private Mock<IMessageBusProvider> messageBusProviderMock;

        [SetUp]
        public void Setup()
        {
            kernel = new MockingKernel();
            messageBusProviderMock = new Mock<IMessageBusProvider>();
            kernel.Bind<IMessageBusProvider>().ToConstant(messageBusProviderMock.Object);

            messageCorpController = kernel.Get<IMessageCorpController>();

            disposables.Add(kernel);
        }

        [Test]
        public async Task TestGenericPostHandler()
        {
            // Arrange
            var context1 = CreateHttpListenerContext("Request Content 1");
            var context2 = CreateHttpListenerContext("Request Content 2");

            // Act
            var responseTask1 = Task.Run(() => InvokeGenericPostHandler(messageCorpController, context1));
            var responseTask2 = Task.Run(() => InvokeGenericPostHandler(messageCorpController, context2));

            await Task.WhenAll(responseTask1, responseTask2);

            // Assert
            var response1 = await responseTask1;
            var response2 = await responseTask2;

            Assert.That(response1 != null);
            Assert.That(response2 != null);

            // Add more assertions as needed based on the expected behavior of your method
        }

        private async Task<HttpListenerResponse> InvokeGenericPostHandler(IMessageCorpController controller, HttpListenerContext context)
        {
            // Use reflection to access the private GenericPostHandler method
            var methodInfo = typeof(MessageCorpController).GetMethod("GenericPostHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            return await (Task<HttpListenerResponse>)methodInfo!.Invoke(controller, new object[] { context })!;
        }

        private HttpListenerContext CreateHttpListenerContext(string content)
        {
            var requestStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            var contextMock = new Mock<HttpListenerContext>();
            var requestMock = new Mock<HttpListenerRequest>();
            var responseMock = new Mock<HttpListenerResponse>();

            requestMock.SetupGet(r => r.HasEntityBody).Returns(true);
            requestMock.SetupGet(r => r.InputStream).Returns(requestStream);
            contextMock.SetupGet(c => c.Request).Returns(requestMock.Object);
            contextMock.SetupGet(c => c.Response).Returns(responseMock.Object);

            return contextMock.Object;
        }
        */
    }
}
