using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System;
using System.Text.Json.Serialization;
using WT.DirectLogistics.Application;
using Microsoft.OpenApi.Models;
using WT.DirectLogistics.Infrastructure;
using WT.DirectLogistics.Application.Common.Interfaces;
using WT.DirectLogistics.WebAPI.Services;
using WT.DirectLogistics.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using System.Net;
using Microsoft.Extensions.Hosting;
using Exceptionless;
using WT.DirectLogistics.Infrastructure.Identity;
using WT.DirectLogistics.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureServices

builder.Logging.AddExceptionless();
builder.Services.AddExceptionless();
builder.Services.AddHttpClient();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

builder.Services.AddControllersWithViews(options =>
    options.Filters.Add<ApiExceptionFilterAttribute>())
        .AddFluentValidation().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new DateTimeConverter()));


// Customise default API behaviour
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownProxies.Add(IPAddress.Parse("122.115.40.7"));
});

builder.Services.AddCors(options =>
{
    //wuliujia2018为三方合作网站，非物通域名
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("https://*.chinawutong.com", "http://*.chinawutong.com", "http://*.wuliujia2018.com")
                   .AllowCredentials()
                   .AllowAnyHeader()
                   .SetIsOriginAllowedToAllowWildcardSubdomains();
        });
});
builder.Services.AddDistributedRateLimiting();
builder.Services.AddMemoryCache();
builder.Services.Configure<ClientRateLimitOptions>(builder.Configuration.GetSection("ClientRateLimiting"));
builder.Services.Configure<ClientRateLimitPolicies>(builder.Configuration.GetSection("ClientRateLimitPolicies"));
builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

//npgsql 版本更新之后，时间转换
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
});

#endregion


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // get the ClientPolicyStore instance
    var clientPolicyStore = scope.ServiceProvider.GetRequiredService<IClientPolicyStore>();

    // seed Client data from appsettings
    await clientPolicyStore.SeedAsync();

}


#region Configure
var hostEnvironment = app.Services.GetRequiredService<IHostEnvironment>();
if (hostEnvironment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI v1"));
}
app.UseStaticFiles();
app.UseClientRateLimiting();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseExceptionless();
app.UseHealthChecks("/health");
app.UseMiddleware<IdentityMiddleware>();
app.UseMiddleware<LoginServers>();
//app.UseBadWordsMiddleware(new BadWordsOptions
//{
//    conn = Configuration.GetConnectionString("WT_MS_BADWORDS")
//});
app.UseRouting();
app.UseCors();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


#endregion



await app.RunAsync();



public class DateTimeConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return DateTime.Parse(reader.GetString());
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}

