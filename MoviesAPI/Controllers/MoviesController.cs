using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using MoviesAPI.Services;
using System.Linq.Dynamic.Core;

namespace MoviesAPI.Controllers
{
    [ApiController]
    [Route("api/movies")]
    public class MoviesController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly IFileManager fileManager;
        private readonly ILogger<MoviesController> logger;
        private readonly string container = "movies";

        public MoviesController(ApplicationDbContext context, IMapper mapper, IFileManager fileManager, ILogger<MoviesController> logger) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileManager = fileManager;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<MoviesIndexDTO>> Get()
        {
            var top = 5;
            var today = DateTime.Today;

            var nextLaunches = await context.Movies.
                Where(a => a.LaunchDate > today)
                .OrderBy(a => a.LaunchDate)
                .Take(top)
                .ToListAsync();

            var onTheater = await context.Movies.
                Where(a => a.InTheaters)
                .Take(top).
                ToListAsync();

            var result = new MoviesIndexDTO()
            {
                NextLaunches = mapper.Map<List<MovieDTO>>(nextLaunches),
                OnTheaters = mapper.Map<List<MovieDTO>>(onTheater),
            };



            var movies = await context.Movies.ToListAsync();
            return result;
        }

        [HttpGet("filter")]
        public async Task<ActionResult<List<MovieDTO>>> Filter([FromQuery] MovieFilterDTO movieFilterDTO)
        {

            var moviesQueryable = context.Movies.AsQueryable();

            //ejecucion diferida

            if (!string.IsNullOrEmpty(movieFilterDTO.Title))
            {
                moviesQueryable = moviesQueryable.Where(a => a.Title.Contains(movieFilterDTO.Title));
            }
            if (movieFilterDTO.InTheaters)
            {
                moviesQueryable = moviesQueryable.Where(a => a.InTheaters);
            }
            if (movieFilterDTO.NextLaunches)
            {
                var today = DateTime.Today;
                moviesQueryable = moviesQueryable.Where(a => a.LaunchDate > today);
            }
            if (movieFilterDTO.GenreId != 0)
            {

                moviesQueryable = moviesQueryable.Where(a => a.MoviesGenres.Select(a => a.GenreId).Contains(movieFilterDTO.GenreId));
            }
            if (!string.IsNullOrEmpty(movieFilterDTO.SortField))
            {

                var sortOrder = movieFilterDTO.SortOrder ? "ascending" : "descending";
                try
                {
                    moviesQueryable = moviesQueryable.OrderBy($"{movieFilterDTO.SortField} {sortOrder}");

                }
                catch (Exception ex)
                {
                    logger.LogError(ex.Message, ex);
                    //return BadRequest(ex.Message);
                }
            }

            await HttpContext.InsertPaginationParameters(moviesQueryable, movieFilterDTO.RecordsQuantityPerPage);
            var movies = await moviesQueryable.Paginate(movieFilterDTO.Pagination).ToListAsync();
            return mapper.Map<List<MovieDTO>>(movies);
        }

        [HttpGet("{id}", Name = "getMovie")]
        public async Task<ActionResult<MovieDetailsDTO>> Get(int id)
        {

            var movie = await context.Movies
                .Include(a => a.MoviesGenres).ThenInclude(b => b.Genre)
                .Include(a => a.MoviesActors).ThenInclude(b => b.Actor)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (movie is null)
            {
                return NotFound();
            }
            movie.MoviesActors = movie.MoviesActors.OrderBy(a => a.Order).ToList();

            return mapper.Map<MovieDetailsDTO>(movie);
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
            AssignOrderToActors(movie);
            context.Add(movie);
            await context.SaveChangesAsync();
            var dto = mapper.Map<MovieDTO>(movie);
            return new CreatedAtRouteResult("getMovie", new { id = movie.Id }, dto);

        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromForm] MovieCreationDTO movieCreationDTO)
        {
            var movieDB = await context.Movies
                .Include(a => a.MoviesGenres)
                .Include(a => a.MoviesActors)
                .FirstOrDefaultAsync(a => a.Id == id);

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
            AssignOrderToActors(movieDB);

            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> Patch(int id, [FromBody] JsonPatchDocument<MoviePatchDTO> patchDocument)
        {

            return await Patch<Movie, MoviePatchDTO>(id, patchDocument);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
           
            return await Delete<Movie>(id);
        }

        private void AssignOrderToActors(Movie movie)
        {

            if (movie.MoviesActors is not null)
            {

                for (int i = 0; i < movie.MoviesActors.Count; i++)
                {
                    movie.MoviesActors[i].Order = i;
                }
            }

        }
    }
}
