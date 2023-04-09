using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using Newtonsoft.Json;

namespace MoviesAPI.Tests.IntegrationTests
{
    [TestClass]
    public class GenresControllerTests:TestBase
    {
        private static readonly string url = "/api/genres";

        [TestMethod]
        public async Task GetAllGenresEmptyList() { 
        
            var dbName = Guid.NewGuid().ToString(); 
            var factory = BuildWebAppFactory(dbName);

            var client = factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var genres = JsonConvert.DeserializeObject<List<GenreDTO>>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(0, genres.Count()); 
        }

        [TestMethod]
        public async Task GetAllGenres() {
            var dbName = Guid.NewGuid().ToString();
            var factory = BuildWebAppFactory(dbName);

            var context= BuildContext(dbName);
            context.Add(new Genre() {Name="Genre 1" });
            context.Add(new Genre() { Name = "Genre 2" });
           await  context.SaveChangesAsync();

            var client = factory.CreateClient();
            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            var genres = JsonConvert.DeserializeObject<List<GenreDTO>>(await response.Content.ReadAsStringAsync());

            Assert.AreEqual(2, genres.Count());

        }

        [TestMethod]
        public async Task DeleteGenre() { 
            var dbName = Guid.NewGuid().ToString();
            var factory = BuildWebAppFactory(dbName, true);

            var context = BuildContext(dbName);
            context.Add(new Genre() { Name = "Genre 1" });
            await context.SaveChangesAsync();

            var client = factory.CreateClient();
            var response = await client.DeleteAsync($"{url}/1");
            response.EnsureSuccessStatusCode();

            var context2 = BuildContext(dbName);
            var exist = await context2.Genres.AnyAsync();
            Assert.IsFalse(exist);


        }

        [TestMethod]
        public async Task DeleteGenreReturn401()
        {
            var dbName = Guid.NewGuid().ToString();
            var factory = BuildWebAppFactory(dbName, ignoreSecurity:false);

            var client = factory.CreateClient();
            var response = await client.DeleteAsync($"{url}/1");
            Assert.AreEqual("Unauthorized", response.ReasonPhrase);



        }
    }
}
