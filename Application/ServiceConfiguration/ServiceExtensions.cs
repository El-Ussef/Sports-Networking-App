using System.Reflection;
using Application.Common.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServiceConfiguration;

public static class ServiceExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        //this needs MediatR 11 with MediatR.Extensions.Microsoft.DependencyInjection
        //services.AddMediatR(Assembly.GetExecutingAssembly()); 
        services.AddScoped<UploadFileHelper>();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    }
}