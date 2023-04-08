using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace MoviesAPI.Tests
{
    public class FalseUserFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity(new List<Claim> { 
            
                new Claim(ClaimTypes.Email,"email@email.com"),
                new Claim(ClaimTypes.Name,"email@email.com"),
                new Claim(ClaimTypes.NameIdentifier,"5673b8cf-12de-44f6-92ad-fae4a77932ad"),

            }, "test"));

            await next();
        }
    }
}
