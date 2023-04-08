using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using NetTopologySuite.Geometries;

namespace MoviesAPI.Controllers
{
    [Route("api/theaters/")]
    [ApiController]
    public class TheatersController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly GeometryFactory geometryFactory;

        public TheatersController(ApplicationDbContext context, IMapper mapper, GeometryFactory geometryFactory) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.geometryFactory = geometryFactory;
        }

        [HttpGet]
        public async Task<ActionResult<List<TheaterDTO>>> Get()
        {

            return await Get<Theater, TheaterDTO>();
        }

        [HttpGet("{id:int}", Name = "getTheater")]
        public async Task<ActionResult<TheaterDTO>> Get(int id)
        {

            return await Get<Theater, TheaterDTO>(id);
        }

        [HttpGet("NearBy")]
        public async Task<ActionResult<List<TheaterNearDTO>>> NearBy([FromQuery] TheatersNearFilterDTO theatersNearFilterDTO) { 
        
            var userLocation = geometryFactory.CreatePoint(new Coordinate(theatersNearFilterDTO.Longitude, theatersNearFilterDTO.Latitude));
            var theaters = await context.Theaters.OrderBy(a => a.Location.Distance(userLocation)).
                Where(a => a.Location.IsWithinDistance(userLocation, theatersNearFilterDTO.DistanceInKms * 1000))
                .Select(a => new TheaterNearDTO
                {

                    Id = a.Id,
                    DistanceInMeters = Math.Round(a.Location.Distance(userLocation)),
                    Latitude = a.Location.Y,
                    Longitude = a.Location.X,
                    Name = a.Name,

                }).ToListAsync();

            return theaters;

        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] TheaterCreationDTO theaterCreationDTO)
        {
            return await Post<TheaterCreationDTO, Theater, TheaterDTO>(theaterCreationDTO, "getTheater");
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] TheaterCreationDTO theaterCreationDTO) {

            return await Put<TheaterCreationDTO, Theater>(id, theaterCreationDTO);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) { 
        
            return await Delete<Theater>(id);   
        }
    }
}
