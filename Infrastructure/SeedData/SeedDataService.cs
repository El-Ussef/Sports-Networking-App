using System.Reflection.Emit;
using System.Text.Json;
using Application.Contracts;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.SeedData;

public interface ISeedDataBase
{
    Task SeedAll();
}

public class SeedDataService : ISeedDataBase
{
    private readonly IApplicationDbContext _dbContext;
    
    public SeedDataService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task SeedAll()
    {
        try
        {
            await SeedCountriesData(_dbContext);
            await SeedRepositoryData(_dbContext);
            await SeedSportData(_dbContext);
            await SeedSpecialitiesData(_dbContext);
            await SeedServiceType(_dbContext);
            await SeedServiceExample(_dbContext);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException);
            throw;
        }
       
    }

    private async Task SeedCountriesData(IApplicationDbContext dbContext)
    {
        // Get the current working directory of the application
        string currentPath = Directory.GetCurrentDirectory();
    
        // Combine the current directory with the relative path of your JSON file
        //string filePath = Path.Combine(currentPath,"Infrastructure", "RepositoryData", "Countries.json");
        string filePath = "../Infrastructure/RepositoryData/Countries.json";


        if (File.Exists(filePath))
        {
            var jsonData = File.ReadAllText(filePath);
            var countries = JsonSerializer.Deserialize<List<Country>>(jsonData);
            if (countries != null && !dbContext.Countries.Any() && countries.Any())
            {
                await dbContext.Countries.AddRangeAsync(countries);
                await _dbContext.SaveChangesAsync();
            }
        }

    }

    private async Task SeedSpecialitiesData(IApplicationDbContext dbContext)
    {
        if (!dbContext.Specialities.Any())
        {
            var specialities = new List<Speciality>()
            {
                new Speciality()
                {
                    Label = "Athlete",
                    UserType = UserType.Athlete
                },
                new Speciality()
                {
                    Label = "Coach",
                    UserType = UserType.Coach
                },
                new Speciality()
                {//include Nutritionist
                    Label = "Medical and Health",
                    UserType = UserType.MedicalAndHealth
                },
                new Speciality()
                {
                    Label = "Manager",
                    UserType = UserType.Manager
                },
                new Speciality()
                {
                    Label = "Sports equipment",
                    UserType = UserType.Shop
                },
                // new Speciality()
                // {
                //     Label = "Nutritionist",
                //     UserType = UserType.MedicalAndHealth
                // },
                new Speciality()
                {
                    Label = "Club",
                    UserType = UserType.Club
                }
            };
            await dbContext.Specialities.AddRangeAsync(specialities);

        }
    }

    public async Task SeedRepositoryData(IApplicationDbContext dbContext)
    {
        if (dbContext.Countries.Any())
        {
            var spain = await dbContext.Countries.FirstAsync(c => c.CountryCode == "ES");

            if (!dbContext.Cities.Any())
            {
                spain.Regions = new List<Region>()
                {
                new Region
                {
                    Label = "Andalusia",
                    Provinces = new List<Province>()
                    {
                        new Province
                        {
                            Label = "Almeria",
                            Cities = new List<City>()
                            {
                                new City
                                {
                                    Label = "almeria",
                                    Country = spain
                                },
                            }
                        },
                        new Province
                        {
                            Label = "Cádiz",
                            Cities = new List<City>()
                            {
                                new City
                                {
                                    Label = "Algeciras",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "Arcos de la Frontera",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "Cádiz",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "Chiclana de la Frontera",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "El Puerto de Santa María",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "Jerez de la Frontera",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "La Línea",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "Puerto Real",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "San Fernando",
                                    Country = spain

                                },
                                new City
                                {
                                    Label = "Sanlúcar de Barrameda",
                                    Country = spain

                                }
                            }
                        }
                    }
                } 
            
            
            };
            }
            
            
            dbContext.Countries.Update(spain);
            await _dbContext.SaveChangesAsync();

        }
        
    }

    public async Task SeedSportData(IApplicationDbContext dbContext)
    {
        if (!dbContext.SportCategories.Any())
        {
            var sportCategory = new List<SportCategory>
                 {
            new SportCategory{
            Label = "Running",
            Sports = new List<Sport>
            {
                new Sport
                {
                    Name = "Endurance",
                    Hints = new []{"5k,10K,Marathon..."},
                },
                new Sport
                {
                    Name = "Sprint"
                },
                new Sport
                {
                    Name = "Hurdles"
                }
            }
            },
            new SportCategory
            {
                Label = "Acrobatic arts",
                Sports = new List<Sport>
                {
                    new Sport
                    {
                        Name = "Gymnastics"
                    },
                }
                
            },
            new SportCategory
            {
                Label = "Archery"
            },
            new SportCategory
            {
                Label = "Board sports",
                Sports = new List<Sport>
                {
                    new Sport
                    {
                        Name = "Skateboarding"
                    },
                    new Sport
                    {
                        Name = "Snowboarding"
                    },
                    new Sport
                    {
                        Name = "Surfing"
                    }
                    
                }
            },
            new SportCategory
            {
                Label = "Cycling"
            },
            new SportCategory
            {
                Label = "martial arts"
            },
            new SportCategory
            {
                Label = "Flying disc sports"
            },
            new SportCategory
            {
                Label = "Gymnastic"
            },
            new SportCategory
            {
                Label = "Ice sports"
            },
            new SportCategory
            {
                Label = "Mixed discipline"
            },
            new SportCategory
            {
                Label = "Running"
            },
            new SportCategory
            {
                Label = "Sailing"
            },
            new SportCategory
            {
                Label = "Shooting sports"
            },
            new SportCategory
            {
                Label = "Aquatic sports"
            },
            new SportCategory
            {
                Label = "Weightlifting"
            },
            new SportCategory
            {
                Label = "Motorsports"
            },
            new SportCategory
            {
                Label = "Ball games"
            },
            new SportCategory
            {
                Label = "Esports"
            },
            new SportCategory
            {
                Label = "Strategy board games"
            }
        };
            await dbContext.SportCategories.AddRangeAsync(sportCategory);
        }
    }

    public async Task SeedServiceType(IApplicationDbContext dbContext)
    {
        if (!dbContext.ServiceTypes.Any())
        {
            var serviceTypes = new List<ServiceType>()
            {
                new ServiceType()
                {
                    Label = "Branding"
                },
                new ServiceType()
                {
                    Label = "Personal Training"
                },
                new ServiceType()
                {
                    Label = "Social Media Promotion"
                },
                new ServiceType()
                {
                    Label = "Event Participation"
                },
                new ServiceType()
                {
                    Label = "Endorsements"
                },
                new ServiceType()
                {
                    Label = "Consultation"
                }
            };
            await dbContext.ServiceTypes.AddRangeAsync(serviceTypes);

        }
    }
    
    public async Task SeedServiceExample(IApplicationDbContext dbContext)
    {
        if (!dbContext.ServiceTypes.Any())
        {
            var ServiceExamples = new List<ServiceExample>()
            {
                new ServiceExample()
                {
                    Label = "Put your logo in my T-shirt"
                },
                new ServiceExample()
                {
                    Label = "Publicity in RS"
                },
                new ServiceExample()
                {
                    Label = "Train with me "
                },
                new ServiceExample()
                {
                    Label = "Add you own"
                }
            };
            await dbContext.ServiceExamples.AddRangeAsync(ServiceExamples);

        }
    }
    
}