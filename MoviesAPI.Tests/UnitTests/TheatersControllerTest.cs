using MoviesAPI.Controllers;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoviesAPI.Tests.UnitTests
{
    [TestClass]
    public class TheatersControllerTest : TestBase
    {
        [TestMethod]
        public async Task GetTheatetsNearBy() { 
        
            var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

            using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false)) {

                var theater = new List<Theater>() {  
                    new Theater{Name = "Guayama", Location = geometryFactory.CreatePoint(new Coordinate(-66.098658, 17.98054))},
                };
                context.AddRange(theater);
                await context.SaveChangesAsync();   
            }

            var filter = new TheatersNearFilterDTO(){DistanceInKms=5,Latitude = 17.98054,Longitude= -66.098658 };

            using (var context = LocalDbDatabaseInitializer.GetDbContextLocalDb(false))
            {
                var mapper = ConfigureAutoMapper();
                var controller = new TheatersController(context, mapper, geometryFactory);
                var response = await controller.NearBy(filter);
                var value = response.Value;

                Assert.AreEqual(1, value.Count());
               
            }


        }
    }
}
