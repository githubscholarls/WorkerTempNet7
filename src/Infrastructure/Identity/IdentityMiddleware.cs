using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace WT.DirectLogistics.Infrastructure.Identity
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }

    public class IdentityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        private const string BearSchemes = "Bearer";
        private const string BasicSchemes = "Basic";

        private static readonly List<(string ak, string sk)> Clients = new()
        {
            ("fJPEoLkVYo0/b2dpPb77byOBx2BkIrEB3WqgOWNsChh7", "nO5sN7aJZWYmqKc1bDdHg6EHxdyLnymJvos2xGXgCw==")
        };

        public IdentityMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {

            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                var header = authorizationHeader.Trim();

                if (header.StartsWith(BearSchemes))
                {
                    var token = header[BearSchemes.Length..].Trim();

                    if (!string.IsNullOrEmpty(token))
                    {
                        AttachUserToContext(context, token);
                    }
                }

                if (header.StartsWith(BasicSchemes))
                {
                    var token = header[BasicSchemes.Length..].Trim();

                    if (!string.IsNullOrEmpty(token))
                    {
                        var buffer = Convert.FromBase64String(token);
                        token = Encoding.UTF8.GetString(buffer);

                        var ak = token.Split(":")?.FirstOrDefault();
                        var sk = token.Split(":").LastOrDefault();
                        var flag = Clients.Where(x => x.ak == ak && x.sk == sk).Any();

                        if (flag)
                        {
                            ClaimsIdentity identity = new(new[] {
                                new Claim(ClaimTypes.NameIdentifier,"0")
                            });
                            var principal = new ClaimsPrincipal(identity);
                            context.User = principal;
                        }
                    }
                }
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out _);

                context.User = claimsPrincipal;
            
        }
    }

}
