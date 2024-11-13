using System.Reflection;
using Application.Contracts;
using Application.Mappings;
using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Identity.Entities;
using Infrastructure.Identity.Services;
using Infrastructure.SeedData;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ServiceConfiguration;

public static class ServiceExtensions
{
    public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
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

        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddAutoMapper(Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(IMapFrom<>)));
        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        //services.AddScoped(typeof(IUserServiceFactory<>), typeof(UserServiceFactory<>));
        services.AddScoped<IUserServiceFactory, UserServiceFactory>();
        services.AddTransient<IFileStorageService, FileStorageService>(provider =>
            new FileStorageService(Path.Combine(modifiedPath, "Images")));

        services.AddScoped<AthleteService>();
        services.AddScoped<CoachService>();
        services.AddScoped<ClubService>();
        services.AddScoped<ManagerService>();
        services.AddScoped<MedicalAndHealthService>();

        services.AddSignalR();
        // services.AddScoped<IUserService<Athlete>, AthleteService>();
        // services.AddScoped<IUserService<Coach>, CoachService>();
        // services.AddScoped<IUserService<Club>,ClubService>();
        // services.AddScoped<IUserService<Manager>,ManagerService>();
        // services.AddScoped<IUserService<MedicalStaff>,MedicalStaffService>();
        // services.AddScoped<IUserService<Nutritionist>,NutritionistService>();

        //services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());
        //services.AddScoped<ITenantService, TenantService>();
        services.AddTransient<IDateTimeService, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ISeedDataBase, SeedDataService>();
        services.AddTransient<IJwtTokenService,JwtTokenService>();
        services.AddTransient<IMessageService,MessageService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddIdentityCore<ApplicationUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }
}