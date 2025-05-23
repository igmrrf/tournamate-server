using System;
using System.Text;
using Api.Middleware;
using Infrastructure.Extensions;
using Infrastructure.InternalServices.EmailService;
using Infrastructure.InternalServices.Jwt;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using UseCase.Exceptions;
using static UseCase.Commands.TournamentCommand.SaveToDraft;



var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: "Logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}"
    )
    .MinimumLevel.Debug()
    .CreateLogger();
builder.Host.UseSerilog();

var configuration = builder.Configuration;

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
// Add Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options
            .UseMySQL(builder.Configuration.GetConnectionString("ApplicationDbContext")) );

builder.Services.AddMediatR(mR => mR.RegisterServicesFromAssemblies(typeof(SaveToDraftCommand).Assembly));

builder.Services.AddScoped<HttpClient>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT"));

builder.Services.Configure<EmailOptionProvider>(builder.Configuration.GetSection("EmailOptionProvider"));

builder.Services.AddRepository();
builder.Services.AddServices(configuration);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdmin", policy =>
        policy.Requirements.Add(new TournamentRoleRequirement("Admin")));

    options.AddPolicy("RequireReferee", policy =>
        policy.Requirements.Add(new TournamentRoleRequirement("Referee")));

    options.AddPolicy("RequireManager", policy =>
        policy.Requirements.Add(new TournamentRoleRequirement("Manager")));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Game-Manager API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


var app = builder.Build();
// Use middleware for exception handling
app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}


// Enable CORS
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
// Serve static files
app.UseStaticFiles();
// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();
// Configure routing
app.UseRouting();
// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();
// Configure the HTTP request pipeline for development
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.MapOpenApi();
    app.UseSwaggerUI();
}
// Map controllers to endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
// Apply database migrations
//MigrationExtensions.ApplyMigration(app.Services, !builder.Environment.IsProduction());
// Run the application
app.Run();

