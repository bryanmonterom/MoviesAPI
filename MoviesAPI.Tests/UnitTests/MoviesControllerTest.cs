using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class MoviesControllerTest : TestBase
    {
        private string CreateTestData()
        {
            var databaseName = Guid.NewGuid().ToString();
            var context = BuildContext(databaseName);
            var genero = new Genre() { Name = "genre 1" };

            var peliculas = new List<Movie>()
            {
                new Movie(){Title = "Película 1", LaunchDate = new DateTime(2010, 1,1), InTheaters = false},
                new Movie(){Title = "No estrenada", LaunchDate = DateTime.Today.AddDays(1), InTheaters = false},
                new Movie(){Title = "Película en Cines", LaunchDate = DateTime.Today.AddDays(-1), InTheaters = true}
            };

            var peliculaConGenero = new Movie()
            {
                Title = "Película con Género",
                LaunchDate = new DateTime(2010, 1, 1),
                InTheaters = false
            };
            peliculas.Add(peliculaConGenero);

            context.Add(genero);
            context.AddRange(peliculas);
            context.SaveChanges();

            var peliculaGenero = new MoviesGenres() { GenreId = genero.Id, MovieId = peliculaConGenero.Id };
            context.Add(peliculaGenero);
            context.SaveChanges();

            return databaseName;
        }

        [TestMethod]
        public async Task FilterByTitle()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var movieName = "Película 1";

            var fitlerDTO = new MovieFilterDTO() { Title = movieName, RecordsQuantityPerPage = 10 };
            var response = await controller.Filter(fitlerDTO);

            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual(movieName, movies[0].Title);


        }
        [TestMethod]

        public async Task FilterOnTheaters()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);
            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var movieName = "Película en Cines";

            var fitlerDTO = new MovieFilterDTO() { InTheaters = true, RecordsQuantityPerPage = 10 };
            var response = await controller.Filter(fitlerDTO);

            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual(movieName, movies[0].Title);

        }
        [TestMethod]
        public async Task FilterNextLaunches()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);
            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var movieName = "No estrenada";

            var fitlerDTO = new MovieFilterDTO() { NextLaunches = true, RecordsQuantityPerPage = 10 };
            var response = await controller.Filter(fitlerDTO);

            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual(movieName, movies[0].Title);

        }
        [TestMethod]

        public async Task FilterByGenre()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var movieName = "Película con Género";

            var genreId = context.Genres.Select(x => x.Id).First();

            var fitlerDTO = new MovieFilterDTO() { GenreId = genreId, RecordsQuantityPerPage = 10 };
            var response = await controller.Filter(fitlerDTO);

            var movies = response.Value;
            Assert.AreEqual(1, movies.Count);
            Assert.AreEqual(movieName, movies[0].Title);

        }
        [TestMethod]
        public async Task SortTitleAscending()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            var fitlerDTO = new MovieFilterDTO() { SortField = "Title", SortOrder = true };
            var response = await controller.Filter(fitlerDTO);
            var movies = mapper.Map<List<Movie>>(response.Value);

            var context2 = BuildContext(dbName);
            var moviesDB = context2.Movies.ToList().OrderBy(a => a.Title).ToList();

            Assert.AreEqual(moviesDB.Count, movies.Count);

            for (int i = 0; i < movies.Count; i++)
            {
                var movieController = movies[i];
                var movieDB = moviesDB[i];

                Assert.AreEqual(movieController.Id, movieDB.Id);
            }
        }


        [TestMethod]
        public async Task SortTitleDescending()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);

            var controller = new MoviesController(context, mapper, null, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            var fitlerDTO = new MovieFilterDTO() { SortField = "Title", SortOrder = false };
            var response = await controller.Filter(fitlerDTO);
            var movies = mapper.Map<List<Movie>>(response.Value);

            var context2 = BuildContext(dbName);
            var moviesDB = context2.Movies.ToList().OrderByDescending(a => a.Title).ToList();

            Assert.AreEqual(moviesDB.Count, movies.Count);

            for (int i = 0; i < movies.Count; i++)
            {
                var movieController = movies[i];
                var movieDB = moviesDB[i];

                Assert.AreEqual(movieController.Id, movieDB.Id);
            }
        }

        [TestMethod]
        public async Task SortByWrongField()
        {

            var dbName = CreateTestData();
            var mapper = ConfigureAutoMapper();
            var context = BuildContext(dbName);

            var logger = new Mock<ILogger<MoviesController>>();


            var controller = new MoviesController(context, mapper, null, logger.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            var fitlerDTO = new MovieFilterDTO() { SortField = "Titless", SortOrder = false };

            var response = await controller.Filter(fitlerDTO);
            var movies = response.Value;

            var context2 = BuildContext(dbName);
            var moviesDB = await context2.Movies.ToListAsync();
            Assert.AreEqual(moviesDB.Count, movies.Count);
            Assert.AreEqual(1, logger.Invocations.Count);




        }
    }
}
