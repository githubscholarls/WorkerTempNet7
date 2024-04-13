using WT.Trigger.Application.Common.Interfaces;
using WT.Trigger.Infrastructure.Persistence;
using WT.Trigger.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using WT.Trigger.Infrastructure.Kafka;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using WT.Trigger.Infrastructure.InternalHTTP;
using WT.Trigger.Domain.Options;

namespace WT.Trigger.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("WT_PGDB")).UseSnakeCaseNamingConvention());
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<IDateTime, DateTimeService>();

            services.AddSingleton<KafkaClientHandle>();
            services.Configure<ProducerConfig>(configuration.GetSection("Kafka:ProducerSettings"));
            services.Configure<ConsumerConfig>(configuration.GetSection("Kafka:ConsumerSettings"));
            services.AddSingleton<IMQProducer, KafkaProducer>();
            services.AddTransient<IMQCustomer, KafkaCustomer>();


            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
            services.AddTransient<IDbConnection>(sp => new NpgsqlConnection(configuration.GetConnectionString("WT_PGDB")));
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            services.AddHttpClient();
            services.AddHttpClient<IInternalHttpService, InternalHttpService>();


            return services;
        }
    }
}