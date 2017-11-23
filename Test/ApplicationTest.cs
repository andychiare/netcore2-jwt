using JWT;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Test
{
    public class ApplicationTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public ApplicationTest()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.GetFullPath(@"../../.."))
            .AddJsonFile("appsettings.json", optional: false)
            //.AddUserSecrets<Startup>()
            .Build();

            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseConfiguration(configuration));
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task UnAuthorizedAccess()
        {
            var response = await _client.GetAsync("/api/books");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetToken()
        {
            var bodyString = @"{username: ""mario"", password: ""secret""}";
            var response = await _client.PostAsync("/api/token", new StringContent(bodyString, Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            Assert.NotNull((string)responseJson["token"]);
        }

        [Fact]
        public async Task GetBooksWithoutAgeRestrictions()
        {
            var bodyString = @"{username: ""mario"", password: ""secret""}";
            var response = await _client.PostAsync("/api/token", new StringContent(bodyString, Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var token = (string)responseJson["token"];

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var booksResponse = await _client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.OK, booksResponse.StatusCode);

            var bookResponseString = await booksResponse.Content.ReadAsStringAsync();
            var bookResponseJson = JArray.Parse(bookResponseString);
            Assert.Equal(true, bookResponseJson.Count == 4);
        }

        [Fact]
        public async Task GetBooksWithAgeRestrictions()
        {
            var bodyString = @"{username: ""mary"", password: ""barbie""}";
            var response = await _client.PostAsync("/api/token", new StringContent(bodyString, Encoding.UTF8, "application/json"));
            var responseString = await response.Content.ReadAsStringAsync();
            var responseJson = JObject.Parse(responseString);
            var token = (string)responseJson["token"];

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "/api/books");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var booksResponse = await _client.SendAsync(requestMessage);

            Assert.Equal(HttpStatusCode.OK, booksResponse.StatusCode);

            var bookResponseString = await booksResponse.Content.ReadAsStringAsync();
            var bookResponseJson = JArray.Parse(bookResponseString);
            Assert.Equal(true, bookResponseJson.Count == 3);
        }
    }
}
