using AspNetCoreRateLimit;
using ERegServer.DataContext;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.RateLimiting;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<ERegDataContext>(options => options.UseSqlServer(connectionString));


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowOrigin", builder =>
            {
                builder
                    .AllowAnyOrigin() 
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });


        // speed limiting services
        builder.Services.AddOptions();
        builder.Services.AddMemoryCache();

        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
        builder.Services.Configure<IpRateLimitPolicies>(builder.Configuration.GetSection("IpRateLimitPolicies"));

        // speed limiting middleware
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

        builder.Services.AddControllers();

        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((context, configuration) =>
            {
                configuration.Host(builder.Configuration.GetConnectionString("RabbitMQ"));
            });

        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ERegDataContext>();
            dbContext.Database.Migrate();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("AllowOrigin");

        app.UseIpRateLimiting();

        app.UseHttpsRedirection();
        
        app.MapControllers();

        app.Run();
    }
}