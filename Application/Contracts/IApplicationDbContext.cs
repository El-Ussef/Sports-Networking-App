using System.Diagnostics.CodeAnalysis;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Application.Contracts;

public interface IApplicationDbContext
{
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AthleteDocument> AthleteDocuments { get; set; }
    public DbSet<CoachDocument> CoachDocuments { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<ManagerDocument> ManagerDocuments { get; set; }
    public DbSet<MedicalStaffDocument> MedicalStaffDocument { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NutritionistDocument> NutritionistDocuments { get; set; }
    //public DbSet<Offer> Offers { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<TrainingOffer> TrainingOffers { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Video> Videos  { get; set; }
    public DbSet<VideoReport> VideoReports { get; set; }
    
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    
    EntityEntry Entry([NotNull] object entity);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken());
    Task BeginTransactionAsync();
    void RollbackTransaction();

}