using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using WT.DirectLogistics.Application.Common.Interfaces;
using WT.DirectLogistics.Infrastructure.Persistence.Repository;
using System.Text.RegularExpressions;
using WT.DirectLogistics.Infrastructure.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace WT.DirectLogistics.Infrastructure.Services
{
    public class LoginServers
    {
        private readonly RequestDelegate _next;
        public LoginServers(RequestDelegate next) {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.ToString().IndexOf("msem/") != -1)
            {
                var svc = context.RequestServices;
                var _iloginHelper = svc.GetService<ILoginHelper>();
                string token = context.Request.Headers["token"];
                token = string.IsNullOrWhiteSpace(token) ? context.Request.Headers["accesstoken"] : token;
                token = string.IsNullOrWhiteSpace(token) ? context.Request.Cookies["accesstoken"] : token;
                if (await _iloginHelper.IsLoginExists())
                {
                    await _next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
