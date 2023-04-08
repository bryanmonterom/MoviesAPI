using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesAPI.DTOs;
using MoviesAPI.Entities;
using MoviesAPI.Helpers;
using MoviesAPI.Migrations;
using System.Security.Claims;

namespace MoviesAPI.Controllers
{
    // api/peliculas/2/reviews
    [ServiceFilter(typeof(MovieExistAttribute))]
    [Route("api/movies/{movieId:int}/reviews")]
    public class ReviewsController : CustomBaseController
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public ReviewsController(ApplicationDbContext context, IMapper mapper) : base(context, mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet(Name = "getReview")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<List<ReviewDTO>>> Get(int movieId, [FromQuery] PaginationDTO paginationDTO)
        {
            var idUser = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;
            var queryable = context.Reviews.Include(a => a.User).Where(a=> a.MovieId == movieId).AsQueryable();
            return await Get<Review, ReviewDTO>(paginationDTO, queryable);

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Post(int movieId, [FromBody] ReviewCreationDTO reviewCreationDTO)
        {

            var idUser = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;
            var reviewExist = await context.Reviews.AnyAsync(a => a.MovieId == movieId && a.UserId == idUser);
            if (reviewExist)
            {

                return BadRequest("The user already provide a review for this movie");
            }

            //return await Post<ReviewCreationDTO, Review,ReviewDTO>(reviewCreationDTO, "getReview")
            var review = mapper.Map<Review>(reviewCreationDTO);
            review.UserId = idUser;
            review.MovieId = movieId;
            context.Add(review);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{reviewId:int}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Put(int reviewId, int movieId, [FromBody] ReviewCreationDTO reviewCreationDTO)
        {
            var idUser = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;

         
            var reviewDB = await context.Reviews.FirstOrDefaultAsync(a=> a.Id == reviewId);
            if (reviewDB is null)
            {
                return NotFound();
            }
            if (reviewDB.UserId != idUser)
            {

                return Forbid();
            }
            reviewDB = mapper.Map(reviewCreationDTO, reviewDB);
            await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("reviewId:int")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Delete(int reviewId, int movieId, [FromBody] ReviewCreationDTO reviewCreationDTO) {

            var idUser = HttpContext.User.Claims.FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier).Value;

           
            var reviewDB = await context.Reviews.FirstOrDefaultAsync(a => a.MovieId == movieId && a.UserId == idUser);
            if (reviewDB is null)
            {
                return NotFound();
            }
            if (reviewDB.UserId != idUser)
            {

                return Forbid();
            }
            context.Remove(reviewDB);
            await context.SaveChangesAsync();
            return NoContent();
        }

    }
}
