using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Helpers;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests
{
    public class TestBase
    {
       protected string rolAdminId = "9aae0b6d-d50c-4d0a-9b90-2a6873e3845d";
        protected string usuarioAdminId = "5673b8cf-12de-44f6-92ad-fae4a77932ad";
        string userEmail = "bmontero@hotmail.com";
        //To build a db for each test
        protected ApplicationDbContext BuildContext(string dbName)
        {



            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(dbName).Options;
            var dbContext = new ApplicationDbContext(options);
            return dbContext;

        }

        protected IMapper ConfigureAutoMapper()
        {

            var config = new MapperConfiguration(options =>
            {

                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMapperProfiles(geometryFactory));

            });

            return config.CreateMapper();
        }

        protected ControllerContext BuildControllerContext()
        {
            var usuario = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
            new Claim(ClaimTypes.Name as string, userEmail as string),
            new Claim(ClaimTypes.Email as string, userEmail as string),
            new Claim(ClaimTypes.NameIdentifier as string, usuarioAdminId as string),

            }));

            return new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = usuario }
            };
        }
    }
}
