using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Server.HttpSys;
using Microsoft.EntityFrameworkCore;
using Moq;
using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;
using NuGet.Frameworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class ActorsControllerTests : TestBase
    {
        [TestMethod]
        public async Task GetActorsWithPagination()
        {

            //prepare
            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            context.Actors.Add(new Entities.Actor() { Name = "Actor 1" });
            context.Actors.Add(new Entities.Actor() { Name = "Actor 2" });
            context.Actors.Add(new Entities.Actor() { Name = "Actor 3" });
            await context.SaveChangesAsync();


            //execution
            var context2 = BuildContext(dbName);
            var controller = new ActorsController(context2, mapper, null);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var page1 = await controller.Get(new DTOs.PaginationDTO() { Page = 1, RecordsQuantityPerPage = 2 });

            var actorsPage1 = page1.Value;

            //verification

            Assert.AreEqual(2, actorsPage1.Count());

            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            var page2 = await controller.Get(new DTOs.PaginationDTO() { Page = 2, RecordsQuantityPerPage = 2 });
            var actorsPage2 = page2.Value;

            Assert.AreEqual(1, actorsPage2.Count());

            controller.ControllerContext.HttpContext = new DefaultHttpContext();


            var page3 = await controller.Get(new DTOs.PaginationDTO() { Page = 3, RecordsQuantityPerPage = 2 });
            var actorsPage3 = page3.Value;
            Assert.AreEqual(0, actorsPage3.Count());






        }

        [TestMethod]
        public async Task CreateActorWithoutPicture()
        {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var actor = new ActorCreationDTO() { Name = "Bryan", DOB = DateTime.Now };

            var mock = new Mock<IFileManager>();
            mock.Setup(a => a.SaveFile(null, null, null, null))
                .Returns(Task.FromResult("URL"));

            var controller = new ActorsController(context, mapper, mock.Object);
            var response = await controller.Post(actor);

            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);

            var context2 = BuildContext(dbName);
            var list = await context2.Actors.ToListAsync();
            Assert.AreEqual(1, list.Count());
            Assert.IsNull(list[0].Photo);
            Assert.AreEqual(0, mock.Invocations.Count);




        }

        [TestMethod]
        public async Task CreateActorWithPicture()
        {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var content = Encoding.UTF8.GetBytes("image");
            var file = new FormFile(new MemoryStream(content), 0, content.Length, "Data", "image.jpg");
            file.Headers = new HeaderDictionary();
            file.ContentType = "image/jpg";

            var actor = new ActorCreationDTO() { Name = "Bryan", DOB = DateTime.Now, Photo = file };

            var mock = new Mock<IFileManager>();
            mock.Setup(a => a.SaveFile(content, ".jpg", "actors", file.ContentType))
                .Returns(Task.FromResult("URL"));

            var controller = new ActorsController(context, mapper, mock.Object);
            var response = await controller.Post(actor);

            var result = response as CreatedAtRouteResult;
            Assert.AreEqual(201, result.StatusCode);

            var context2 = BuildContext(dbName);
            var list = await context2.Actors.ToListAsync();
            Assert.AreEqual(1, list.Count());
            Assert.AreEqual("URL", list[0].Photo);
            Assert.AreEqual(1, mock.Invocations.Count);




        }

        [TestMethod]
        public async Task PatchReturnNotFoundIfNotExists()
        {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var controller = new ActorsController(context, mapper, null);
            var patchDocument = new JsonPatchDocument<ActorPatchDTO>();
            var response = await controller.Patch(1, patchDocument);

            var result = response as StatusCodeResult;
            Assert.AreEqual(404, result?.StatusCode);

        }

        [TestMethod]
        public async Task PatchUpdateOneField()
        {

            var dbName = Guid.NewGuid().ToString();
            var context = BuildContext(dbName);
            var mapper = ConfigureAutoMapper();

            var dob = DateTime.Now;

            var actor = new Actor() { Name = "Bryan", DOB = dob };
            context.Add(actor);
            await context.SaveChangesAsync();

            var context2 = BuildContext(dbName);
            var controller = new ActorsController(context2, mapper, null);

            var objectValidator = new Mock<IObjectModelValidator>();
            objectValidator.Setup(a => a.Validate
                        (It.IsAny<ActionContext>(),
                        It.IsAny<ValidationStateDictionary>(),
                        It.IsAny<string>(),
                        It.IsAny<object>()));
            controller.ObjectValidator = objectValidator.Object;

            var patchDocument = new JsonPatchDocument<ActorPatchDTO>();
            patchDocument.Operations.Add(new Operation<ActorPatchDTO>("replace", "/name", null, "Bryan2"));
            var response = await controller.Patch(1, patchDocument);

            var result = response as StatusCodeResult;
            Assert.AreEqual(204, result?.StatusCode);

            var context3 = BuildContext(dbName);
            var actorDB = await context3.Actors.FirstAsync();
            Assert.AreEqual("Bryan2", actorDB.Name);
            Assert.AreEqual(dob, actorDB.DOB);




        }
    }
}
