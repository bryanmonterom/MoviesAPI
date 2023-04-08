using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
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
    public class ReviewsControllerTests:TestBase
    {
        [TestMethod]
        public async Task UserCannotCreate2Reviews() { 
        
            var dbName = Guid.NewGuid().ToString(); 
            var context = BuildContext(dbName);
            CreateMovies(dbName);

            var movieId = await context.Movies.Select(a=> a.Id).FirstOrDefaultAsync();
            var review1 = new Review() {MovieId = movieId, UserId = usuarioAdminId, Rate = 5 };
            context.Add(review1);
            await context.SaveChangesAsync();

            var context2 = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var controller = new ReviewsController(context2,mapper);
            controller.ControllerContext = BuildControllerContext();

            var reviewCreationDTO = new ReviewCreationDTO() {  Rate = 5 };
            var response = await controller.Post(movieId,reviewCreationDTO);

            var value = response as IStatusCodeActionResult;
            Assert.AreEqual(400, value.StatusCode.Value);


        }

        [TestMethod]
        public async Task CreateReview() {
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            CreateMovies(dbName);

            var movieId = await context.Movies.Select(a => a.Id).FirstOrDefaultAsync();


            var context2 = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();
            var controller = new ReviewsController(context2, mapper);
            controller.ControllerContext = BuildControllerContext();
            var reviewCreationDTO = new ReviewCreationDTO() { Rate = 5 };


            var response = await controller.Post(movieId, reviewCreationDTO);


            var value = response as NoContentResult;
            Assert.IsNotNull(value);

            var context3 = BuildContext(dbName);
            var reviewDB = context3.Reviews.First();

            Assert.AreEqual(usuarioAdminId, reviewDB.UserId);





        }

        private async void CreateMovies(string dbName) {

            var context = BuildContext(dbName);
            context.Add(new Movie() { Title = "Movie 1" });
            await context.SaveChangesAsync();
        }
    }
}
