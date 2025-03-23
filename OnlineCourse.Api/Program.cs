
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Api.Middlewares;
using OnlineCourse.Core.Entities;
using OnlineCourse.Data;
using OnlineCourse.Service;
using Serilog;
using Serilog.Templates;
using System.Net;

namespace OnlineCourse.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configure Serilog with the settings
            Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.Debug()
            .MinimumLevel.Information()
            .Enrich.FromLogContext()
            .CreateBootstrapLogger();

            try
            {
                #region Service Configuration
                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                var configuration = builder.Configuration;

                builder.Services.AddApplicationInsightsTelemetry();
                builder.Host.UseSerilog((context, services, loggerConfiguration) => loggerConfiguration
                   .ReadFrom.Configuration(context.Configuration)
                   .ReadFrom.Services(services)
                   .WriteTo.Console(new ExpressionTemplate(
                       // Include trace and span ids when present.
                       "[{@t:HH:mm:ss} {@l:u3}{#if @tr is not null} ({substring(@tr,0,4)}:{substring(@sp,0,4)}){#end}] {@m}\n{@x}"))
                   .WriteTo.ApplicationInsights(
                     services.GetRequiredService<TelemetryConfiguration>(), TelemetryConverter.Traces));

                Log.Information("Starting the OnlineCourse API...");

                builder.Services.AddDbContextPool<DbAaf752LmsContext>(options =>
                {
                    options.UseSqlServer(configuration.GetConnectionString("DbContext"), provideroptions => provideroptions.EnableRetryOnFailure());
                    // options.EnableSensitiveDataLogging();
                });

                builder.Services.AddControllers();
                // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
                builder.Services.AddEndpointsApiExplorer();
                builder.Services.AddSwaggerGen();

                builder.Services.AddScoped<ICourseCategoryRepository, CoursCategoryRepository>();
                builder.Services.AddScoped<ICourseCategoryService, CourseCategoryService>();
                builder.Services.AddScoped<ICourseRepository, CourseRepository>();
                builder.Services.AddScoped<ICourseService, CourseService>();


                builder.Services.AddTransient<RequestBodyLoggingMiddleware>();
                builder.Services.AddTransient<ResponseBodyLoggingMiddleware>();

                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("default", policy =>
                    {
                        policy.WithOrigins("http://localhost:4200") // Corrected frontend URL without trailing slash
                              .AllowAnyHeader()
                              .AllowAnyMethod()
                              .AllowCredentials();  // Required for SignalR
                    });
                });
                #endregion

                #region
                var app = builder.Build();

                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = exceptionHandlerPathFeature?.Error;

                        Log.Error(exception, "Unhandled exception occurred. {ExceptionDetails}", exception?.ToString());
                        Console.WriteLine(exception?.ToString());
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsync("An unexpected error occurred. Please try again later.");

                    });
                });

                app.UseMiddleware<RequestResponseLoggingMiddleware>();
                //Enable our custom middleware
                app.UseMiddleware<RequestBodyLoggingMiddleware>();
                app.UseMiddleware<ResponseBodyLoggingMiddleware>();

                // Configure the HTTP request pipeline.
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseCors("default");
                app.UseAuthorization();


                app.MapControllers();

                app.Run();

                #endregion
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host Terminated Unexpectedly");
            }
            finally 
            {
                Log.CloseAndFlush();
            }
        }
    }
}
