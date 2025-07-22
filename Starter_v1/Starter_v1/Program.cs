using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Reflection;
using Starter_v1.BasicInfra;
using Starter_v1.Context;
using Starter_v1.Infra;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog from configuration
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.WithProperty("ApplicationName", "BiomedicaAPI")
        .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);
});

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add health checks with logging
//builder.Services.AddHealthChecks().AddDbContextCheck<AppDbContext>("database");

// Add Database Context
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Add custom logging services
builder.Services.AddScoped<IDataAccessLogger, DataAccessLogger>();

builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new OpenApiInfo {
        Title = "Some Fancy API V1",
        Version = "v1",
        Description = "Comprehensive API for data management",
        Contact = new OpenApiContact
        {
            Name = "API Support",
            Email = "support@fancy-api.com"
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Configure request pipeline with logging
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Some Fancy API V1");
        c.DisplayRequestDuration();
    });
}

// Add health check endpoint
//app.MapHealthChecks("/health");

// Global exception handling with logging
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exceptionFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (exceptionFeature != null)
        {
            logger.LogError(exceptionFeature.Error, 
                "Unhandled exception occurred for {Method} {Path}", 
                context.Request.Method, 
                context.Request.Path);
        }

        context.Response.StatusCode = 500;
        await context.Response.WriteAsync("An error occurred processing your request.");
    });
});

app.MapControllers();

// Log startup completion
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Biomedical API started successfully at {StartupTime}", DateTime.UtcNow);

app.Run();
