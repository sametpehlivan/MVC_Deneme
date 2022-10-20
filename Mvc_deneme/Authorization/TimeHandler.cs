using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;

namespace Mvc_deneme.Authorization
{
    public class TimeHandler : AuthorizationHandler<TimeRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TimeRequirement requirement)
        {
            if (DateTime.Now.Minute > 54)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail(); 
            }
            return Task.CompletedTask;
        }
    }
}
