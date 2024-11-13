using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Contracts;

public interface IApplicationDbContext
{
    // public DbSet<Athlete> Athletes { get; set; }
    // public DbSet<Coach> Coaches { get; set; }
    // public DbSet<Manager> Managers { get; set; }
    // public DbSet<Club> Clubs { get; set; }
    // public DbSet<Nutritionist> Nutritionists { get; set; }
    // public DbSet<MedicalStaff> MedicalStaves { get; set; }
    // public DbSet<Sponsor> Sponsors { get; set; }
    
    public DbSet<AppUser> AppUsers { get; set; }

    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AthleteDocument> AthleteDocuments { get; set; }
    public DbSet<CoachDocument> CoachDocuments { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<ManagerDocument> ManagerDocuments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    //public DbSet<Offer> Offers { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }
    public DbSet<ServiceExample> ServiceExamples { get; set; }

    //public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<SportCategory> SportCategories { get; set; }
    public DbSet<TrainingOffer> TrainingOffers { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Video> Videos  { get; set; }
    public DbSet<VideoReport> VideoReports { get; set; }
    
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Location> Locations { get; set; }

    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    
    EntityEntry Entry([NotNull] object entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken());
    Task BeginTransactionAsync();
    void RollbackTransaction();
    

}