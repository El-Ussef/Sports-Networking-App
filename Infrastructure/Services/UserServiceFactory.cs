using Application.Contracts;
using Domain.Entities;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Identity.Services;

public class UserServiceFactory : IUserServiceFactory
{
    
    private readonly IServiceProvider _serviceProvider;

    public UserServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IUserService GetUserService(UserType userType) 
    {
        // var serviceType = typeof(IUserService<>).MakeGenericType(typeof(T));
        //
        // // Retrieve the service from the service provider
        // var service = _serviceProvider.GetService(serviceType);
        //
        // // Check if the service is of the expected type
        // if (service is IUserService<T> userService)
        // {
        //     return userService;
        // }
        //
        // throw new InvalidOperationException($"Service for type {typeof(T).Name} not found.");
        switch (userType)
        {
            case UserType.Coach:
                return _serviceProvider.GetService<CoachService>();
            case UserType.Athlete:
                return _serviceProvider.GetService<AthleteService>();
            case UserType.MedicalAndHealth:
                return _serviceProvider.GetService<MedicalAndHealthService>();
            case UserType.Club:
                return _serviceProvider.GetService<ClubService>();
            case UserType.Manager:
                return _serviceProvider.GetService<ManagerService>();
            default:
                throw new ArgumentException("Invalid user type");
        }
    }
}