using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertPaginationParameters<T>(this HttpContext httpContext, IQueryable<T> queryable, int recordsPerPage) {

            double qty = await queryable.CountAsync();
            double pagesQuantity = Math.Ceiling(qty/recordsPerPage);
            httpContext.Response.Headers.Add("totalPages", pagesQuantity.ToString());
        }
    }
}
