using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Services;
using System.ComponentModel;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileManager fileManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileManager = fileManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<MovieDTO>>> Get()
        {
            var movies = await context.Movies.ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<MovieDTO>> Get(int id)
        {

            var movie = await context.Movies.FirstOrDefaultAsync(a => a.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            return mapper.Map<MovieDTO>(movie);
        }

        [HttpPost]
        public async Task<ActionResult<MovieCreationDTO>> Post([FromForm] MovieCreationDTO movieCreationDTO)
        {

            var movie = mapper.Map<Movie>(movieCreationDTO);

            if (movieCreationDTO.Poster is not null)
            {
                using (var memoryStream = new MemoryStream())
                {

                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movie.Poster = await fileManager.SaveFile(content, extension, container, movieCreationDTO.Poster.ContentType);
                }
            }
            context.Add(movie);
            await context.SaveChangesAsync();
            var dto = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, dto);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movieDB = await context.Movies.FirstOrDefaultAsync(a => a.Id == id);

            if (movieDB is null)
            {
                return NotFound();
            }

            movieDB = mapper.Map(movieCreationDTO, movieDB);
            if (movieCreationDTO.Poster is not null)
            {
                using (var memoryStream = new MemoryStream())
                {

                    await movieCreationDTO.Poster.CopyToAsync(memoryStream);
                    var content = memoryStream.ToArray();
                    var extension = Path.GetExtension(movieCreationDTO.Poster.FileName);
                    movieDB.Poster = await fileManager.EditFile(content, extension, container, movieCreationDTO.Poster.ContentType, movieDB.Poster);
                }
            }

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {

            if (patchDocument is null)
            {
                return BadRequest();
            }

            var entity = await context.Movies.FirstOrDefaultAsync(a => a.Id == id);

            if (entity is null)
            {

                return NotFound();
            }

            var entityDTO = mapper.Map<MoviePatchDTO>(entity);
            patchDocument.ApplyTo(entityDTO, ModelState);

            var isValid = TryValidateModel(entityDTO);
            if (!isValid)
            {

                return BadRequest(ModelState);
            }

            mapper.Map(entityDTO, entity);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Movies.AnyAsync(actor => actor.Id == id);
            if (!exist)
            {
                return NotFound();
            }
            context.Remove(new Movie { Id = id });
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
