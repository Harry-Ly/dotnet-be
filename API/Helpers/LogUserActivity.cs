using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

// This is an action filter. Has one job: to update LastActive from User that we get from repository. Gives us access to HttpContext
public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next(); // API action is completed, and we get the resultContext with this

        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return; // Sort of redundant

        var userId = resultContext.HttpContext.User.GetUserId();

        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
        var user = await repo.GetUserByIdAsync(userId);
        user.LastActive = DateTime.UtcNow;
        await repo.SaveAllAsync();
    }
}