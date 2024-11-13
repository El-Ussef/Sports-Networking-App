using System.Text;
using Application.BackgroundServices;
using Application.Common.Options;
using Application.Contracts;
using Application.ServiceConfiguration;
using Coravel;
using CoreApi.Services;
using Infrastructure.Context;
using Infrastructure.SeedData;
using Infrastructure.ServiceConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
        true)
    .AddEnvironmentVariables()
    .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.Configure<StorageOption>(builder.Configuration.GetSection("StorageOption"));

builder.Services.AddHttpContextAccessor();
// Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
// builder.Services.AddAuthorization();

builder.Services.AddSignalR();

builder.Services.AddScheduler();

var key = Encoding.ASCII.GetBytes(configuration["JwtKey"]);

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

string _corsPolicy = "_corsPolicy";

string frontOffice = configuration["frontOfficeOrigin"];
string backOffice = configuration["backOfficeOrigin"];

frontOffice = !string.IsNullOrEmpty(frontOffice) ? frontOffice : "http://localhost:4200";
backOffice = !string.IsNullOrEmpty(backOffice) ? backOffice : "http://localhost:4200";

frontOffice = frontOffice ?? "localhost";

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("_corsPolicy", builder =>
    {
        builder.WithOrigins(frontOffice, backOffice)
            .WithMethods("POST", "GET", "PUT", "DELETE")
            .WithHeaders(HeaderNames.Accept,
                HeaderNames.ContentType,
                HeaderNames.Authorization,
                HeaderNames.AccessControlAllowHeaders,
                HeaderNames.AccessControlAllowOrigin)
            .AllowCredentials()
            .SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrWhiteSpace(origin))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(frontOffice) && string.IsNullOrWhiteSpace(backOffice))
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(frontOffice) && frontOffice.ToLower().StartsWith(origin))
                {
                    return true;
                }

                if (!string.IsNullOrEmpty(backOffice) && backOffice.ToLower().StartsWith(origin))
                {
                    return true;
                }

                return false;
            });
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo(){ Title = "web Api", Version = "version 1"});
    //c.OperationFilter<AuthorizationHea>();
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Scheme = "Bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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




var app = builder.Build();

//coravel
app.Services.UseScheduler(s =>
{
    s.Schedule<EventJob>().DailyAt(18, 0);
});

#region Seeding and creating database

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    if (context is null)
    {
        throw new Exception("Database Context Not Found");
    }

    await context.Database.MigrateAsync();

    var seedService = scope.ServiceProvider.GetRequiredService<ISeedDataBase>();
    await seedService.SeedAll();
}

#endregion

#region Pipleline Configuration

app.UseCors(_corsPolicy);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger";
    });
}
app.UseStaticFiles();

var originalPath = Directory.GetCurrentDirectory();

string segmentToRemove = "/CoreApi";

string modifiedPath;

if (originalPath.EndsWith(segmentToRemove))
{
    modifiedPath = originalPath.Substring(0, originalPath.Length - segmentToRemove.Length);
}
else
{
    // Handle cases where the segment is not at the end of the path, or you want to preserve the original path
    modifiedPath = originalPath;
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(modifiedPath, "Images")),
    RequestPath = "/Images"
});

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion