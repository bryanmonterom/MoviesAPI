using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [Route("api/theaters/")]
    [ApiController]
    public class TheatersController : CustomBaseController
    {
        public TheatersController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {

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
