using GenericViewModels.Net;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Generic.ViewModels.Tests.Net
{
    public class HttpClientExtensionsTests
    {
        public class AnItem
        {
            public string Text { get; set; }
        }

        private AnItem[] _testItems = new AnItem[]
        {
            new AnItem { Text = "first" },
            new AnItem { Text = "second" }
        };
        public HttpClientExtensionsTests()
        {

        }

        [Fact]
        public async Task GetItemsAsyncWithItems()
        {
            // arrange
            string testJson = JsonConvert.SerializeObject(_testItems);

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(testJson)
                }));
            var httpClient = new HttpClient(mockMessageHandler.Object);

            IEnumerable<AnItem> items = await HttpClientExtensions.GetItemsAsync<AnItem>(httpClient, new Uri("https://www.cninnovation.com"));
            Assert.Collection(_testItems,
                item1 => Assert.Equal("first", item1.Text),
                item2 => Assert.Equal("second", item2.Text));
        }

        [Fact]
        public async Task GetItemAsyncWithItems()
        {
            // arrange
            string testJson = JsonConvert.SerializeObject(_testItems[1]);

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(Task.FromResult(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(testJson)
                }));
            var httpClient = new HttpClient(mockMessageHandler.Object);

            AnItem item = await HttpClientExtensions.GetItemAsync<AnItem>(httpClient, new Uri("https://www.cninnovation.com"));

            Assert.Equal("second", item.Text);
        }
    }
}
