using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class GenreControllerTests : TestBase
    {
        [TestMethod]
        public async Task GetAllGenres()
        {

            //prepare
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            context.Genres.Add(new Entities.Genre { Name = "Genre 1" });
            context.Genres.Add(new Entities.Genre { Name = "Genre 2" });
            await context.SaveChangesAsync();

            var context2 = BuildContext(dbName);

            //execution

            var controller = new GenresController(context2, mapper);
            var response = await controller.Get();

            //verfication

            var genres = response.Value;
            Assert.AreEqual(2, genres.Count());
        }

        [TestMethod]
        public async Task GetGenreById_NotFound()
        {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var controller = new GenresController(context, mapper);
            var response = await controller.Get(1);

            //verfication

            var result = response.Result as StatusCodeResult;
            Assert.AreEqual(404, result.StatusCode);

        }
        [TestMethod]
        public async Task GetGenreByIdExists()
        {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            context.Genres.Add(new Entities.Genre { Name = "Genre 1" });

            context.Genres.Add(new Entities.Genre { Name = "Genre 1" });
            context.Genres.Add(new Entities.Genre { Name = "Genre 2" });
            await context.SaveChangesAsync();

            var context2 = BuildContext(dbName);

            //execution
            var id = 1;
            var controller = new GenresController(context2, mapper);
            var response = await controller.Get(id);

            var result = response.Value;
            Assert.AreEqual(id, result.Id);

        }

        [TestMethod]
        public async Task CreateGenre() {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var newGenre = new GenreCreationDTO() {Name = "Genre" };
            var controller = new GenresController(context, mapper);

            var response = await controller.Post(newGenre);
            var result = response as CreatedAtRouteResult;

            Assert.IsNotNull(result);

            var context2 = BuildContext(dbName);
            var qty = await context2.Genres.CountAsync();
            Assert.AreEqual(1, qty);

        }

        [TestMethod]
        public async Task UpdateGenre() {

            //prepare
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            context.Genres.Add(new Entities.Genre() { Name ="Genre 1"});
            await context.SaveChangesAsync();

            var context2 = BuildContext(dbName);
            var controller = new GenresController(context2, mapper);

            var genreCreationDTO = new GenreCreationDTO() { Name = "New name" };
            var id = 1;
            //execution
            var response = await controller.Put(id, genreCreationDTO);


            //verification
            var result = response as StatusCodeResult;

            Assert.AreEqual(204, result.StatusCode);

            var context3 = BuildContext(dbName);   
            var exist = await context3.Genres.AnyAsync(a=> a.Name == genreCreationDTO.Name);
            Assert.IsTrue(exist);   


        }

        [TestMethod]
        public async Task DeleteNotExistGenre()
        {

            //prepare
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            //execution

            var controller = new GenresController(context, mapper);

            var response = await controller.Delete(1);

            //verification
            var result = response as StatusCodeResult;

            Assert.AreEqual(404, result.StatusCode);

        }

        [TestMethod]
        public async Task DeleteGenre()
        {

            //prepare
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            context.Genres.Add(new Genre() { Name = "Genre" });
            await context.SaveChangesAsync();


            //execution
            var context2 = BuildContext(dbName);
            var controller = new GenresController(context2, mapper);
            var response = await controller.Delete(1);

            //verification
            var result = response as StatusCodeResult;

            Assert.AreEqual(204, result.StatusCode);

            var context3 = BuildContext(dbName);

            var exist = await context3.Genres.AnyAsync();
            Assert.IsFalse(exist);

        }

    }
}
