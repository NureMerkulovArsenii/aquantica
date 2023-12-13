using Hangfire.Dashboard;

namespace Aquantica.API.Filters;

public class DashBoardAuthFilter: IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        
        return httpContext.User.Identity?.IsAuthenticated ?? false;
    }
}