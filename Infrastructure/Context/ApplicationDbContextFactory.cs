using System.Reflection;
using Infrastructure.Identity.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Context;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        
        var configurationRoot = GetConfigurationRoot();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

        if (configurationRoot.GetValue<bool>("UsePgSql"))
            optionsBuilder.UseNpgsql(configurationRoot.GetConnectionString(nameof(ApplicationDbContext)),
                b => b.MigrationsAssembly(Assembly.GetAssembly(typeof(ApplicationDbContext))?.GetName().FullName));

        //var test = configurationRoot.GetValue<bool>("IsMigration");
        return new ApplicationDbContext(configurationRoot, null,new DateTimeService()); // replace with a suitable default value
    }
    
    public ApplicationDbContext CreateDbContext()
    {
        return CreateDbContext(new string[] { });
    }
    
    public static IConfigurationRoot GetConfigurationRoot()
    {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CoreApi"))
            .AddJsonFile("appsettings.json", false, true)
            //.AddJsonFile($"appsettings-common.{environment}.json", true)
            .AddEnvironmentVariables("SportsNetworkingPlatform_")
            .Build();

        return configuration;
    }
}