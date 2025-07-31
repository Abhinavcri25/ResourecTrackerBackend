using Microsoft.AspNetCore.Authentication.Negotiate;
using ResourceTracker.DAO;
using ResourceTracker.Orchestration;
using Serilog;

namespace ResourecTracker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration().WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day).CreateLogger();

            builder.Host.UseSerilog();


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<SqlConnect>(); // Required for DB connection
            builder.Services.AddScoped<IResourceDAO, ResourceDAO>(); // DAO layer
            builder.Services.AddScoped<IResourceOrchestration, ResourceOrchestration>(); // Orchestration layer

            //builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            //    .AddNegotiate();

            //builder.Services.AddAuthorization(options =>
            //{
            //    // By default, all incoming requests will be authorized according to the default policy.
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});

            builder.Services.AddAuthorization();


            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });


            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors("AllowAngularApp");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
