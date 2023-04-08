using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.Helpers;
using NetTopologySuite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests
{
    public class TestBase
    {
        //To build a db for each test
        protected ApplicationDbContext BuildContext(string dbName) {
            
            var options = new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(dbName).Options;
            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        
        }

        protected IMapper ConfigureAutoMapper() {

            var config = new MapperConfiguration(options =>
            {

                var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
                options.AddProfile(new AutoMapperProfiles(geometryFactory));

            });

            return config.CreateMapper();
        }
    }
}
