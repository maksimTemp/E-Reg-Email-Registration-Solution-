using ERegServer.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using SharedLibrary;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddMassTransit(config =>
        {
            config.AddConsumer<EmailCodeConsumer>();
            config.AddConsumer<ValidatedEmailConsumer>();

            config.UsingRabbitMq((context, configuration) =>
            {
                configuration.Host(builder.Configuration.GetConnectionString("RabbitMQ"));

                configuration.ReceiveEndpoint(QueuesUrls.EmailCodeGenerated, c =>
                {
                    c.ConfigureConsumer<EmailCodeConsumer>(context);
                });
                configuration.ReceiveEndpoint(QueuesUrls.EmailValidated, c =>
                {
                    c.ConfigureConsumer<ValidatedEmailConsumer>(context);
                });
            });

        });

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.Run();
    }
}