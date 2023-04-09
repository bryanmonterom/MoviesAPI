using MoviesAPI.DTOs;
using Newtonsoft.Json;

namespace MoviesAPI.Tests.IntegrationTests
{
    [TestClass]
    public class ReviewControllerTests:TestBase
    {
        private static readonly string url = "/api/movies/1/reviews";

        [TestMethod]
        public async Task GetReviewReturnNotFoundIfMoviesDoesNotExist() { 
        
            var dbName = Guid.NewGuid().ToString();
            var factory = BuildWebAppFactory(dbName);   

            var client = factory.CreateClient();
            var response = await client.GetAsync(url);

            Assert.AreEqual(404, (int)response.StatusCode);
        }
        [TestMethod]
        public async Task GetReviewReturnEmpty() {

            var dbName = Guid.NewGuid().ToString();
            var factory = BuildWebAppFactory(dbName);

            var context= BuildContext(dbName);
            context.Movies.Add(new Entities.Movie() { Title = "Movie1" });
            await context.SaveChangesAsync();

            var client = factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var reviews = JsonConvert.DeserializeObject<List<ReviewDTO>>(await response.Content.ReadAsStringAsync());
            Assert.AreEqual(0, reviews.Count());




        }
    }
}
