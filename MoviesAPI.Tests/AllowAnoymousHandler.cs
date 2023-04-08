﻿using Microsoft.AspNetCore.Authorization;

namespace MoviesAPI.Tests
{
    public class AllowAnoymousHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (var requirement in context.PendingRequirements.ToList())
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
