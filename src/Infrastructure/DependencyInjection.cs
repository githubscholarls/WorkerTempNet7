using CSRedis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using WT.DirectLogistics.Application.Common.Interfaces;
using WT.DirectLogistics.Infrastructure.Persistence;
using WT.DirectLogistics.Infrastructure.Persistence.Repository;
using WT.DirectLogistics.Infrastructure.Services;
using WT.DirectLogistics.Infrastructure.Identity;
using Confluent.Kafka;
using WT.DirectLogistics.Infrastructure.Kafka;
using MySqlConnector;
using MongoDB.Driver;

namespace WT.DirectLogistics.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {          

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddDbContext<MSDbContext>(optinos => optinos.UseSqlServer(configuration.GetConnectionString("WT_MSDB")));
            services.AddScoped<IMSDbContext>(provider=>provider.GetService<MSDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTime, DateTimeService>();
            services.AddSingleton<IDistributedCache>(sp =>
            {
                var csredis = new CSRedisClient(configuration.GetConnectionString("Redis"));
                return new Microsoft.Extensions.Caching.Redis.CSRedisCache(csredis);
            });

            services.AddSingleton(sp => new CSRedisClient(configuration.GetConnectionString("Redis")));
            services.AddCap(o =>
            {
                o.UseKafka(configuration["Kafka:BootstrapServers"]);
                o.UseInMemoryStorage();
            });
            services.AddSingleton<KafkaClientHandle>();
            services.Configure<ProducerConfig>(configuration.GetSection("KafkaCache:ProducerSettings"));
            services.AddSingleton<IMQProducer, KafkaProducer>();
            services.AddSingleton(sp =>
             new MongoClient(configuration.GetSection("SeoUrlMongoDB").GetSection("ConnectionString").Value)
             .GetDatabase(configuration.GetSection("SeoUrlMongoDB").GetSection("DatabaseName").Value));

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddTransient<IDbConnection>(sp => new SqlConnection(configuration.GetConnectionString("WT_MSDB")));
            services.AddTransient(sp=>new MySqlConnection(configuration.GetConnectionString("WT_MYSQL")));
            services.Configure<IdentityConfig>(configuration.GetSection("Identity"));
            services.AddTransient(sp => new MySqlConnection(configuration.GetConnectionString("WT_MYSQL")));
            services.AddScoped<ILoginHelper, LoginHelperRepository>();
            services.AddTransient<IJwtRespository, JwtRespository>();
            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddTransient(sp => new Npgsql.NpgsqlConnection(configuration.GetConnectionString("WT_PG_MAIN_DB")));           


            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            });



            return services;
        }
    }
}