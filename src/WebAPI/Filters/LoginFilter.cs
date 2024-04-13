using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using WT.DirectLogistics.Application.Common.Interfaces;

namespace WT.DirectLogistics.WebAPI.Filters
{
    public class LoginFilter : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            var svc = context.HttpContext.RequestServices;
            var loginRepository = svc.GetRequiredService<ILoginHelper>();
            if (!(await loginRepository.IsLoginExists()))
            {
                var details = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                };

                context.Result = new ObjectResult(details)
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}
