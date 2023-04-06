using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/genres")]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public GenresController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<GenreDTO>>> Get()
        {

            var entities = await context.Genres.ToListAsync();
            var dtos = mapper.Map<List<GenreDTO>>(entities);
            return dtos;
        }

        [HttpGet("{id:int}", Name = "getGenre")]
        public async Task<ActionResult<GenreDTO>> Get(int id)
        {
            var entity = await context.Genres.FirstOrDefaultAsync(genre => genre.Id == id);
            if (entity is null)
            {
                return NotFound();
            }
            var dto = mapper.Map<GenreDTO>(entity);
            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] GenreCreationDTO genreCreationDTO)
        {

            var entity = mapper.Map<Genre>(genreCreationDTO);
            context.Add(entity);
            await context.SaveChangesAsync();
            var dto = mapper.Map<GenreDTO>(entity);
            return new CreatedAtRouteResult("getGenre", new { id = dto.Id }, dto);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] GenreCreationDTO genreCreationDTO)
        {

            var entity = mapper.Map<Genre>(genreCreationDTO);
            entity.Id = id;
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var exist = await context.Genres.AnyAsync(genre => genre.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Genre { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
