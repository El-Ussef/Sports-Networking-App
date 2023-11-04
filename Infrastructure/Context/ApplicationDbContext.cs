using System.Data;
using System.Reflection;
using Application.Contracts;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>,IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;
    private readonly int _tenantId;
    private IDbContextTransaction _currentTransaction;
    
    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AthleteDocument> AthleteDocuments { get; set; }
    public DbSet<CoachDocument> CoachDocuments { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<ManagerDocument> ManagerDocuments { get; set; }
    public DbSet<MedicalStaffDocument> MedicalStaffDocument { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NutritionistDocument> NutritionistDocuments { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<TrainingOffer> TrainingOffers { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<VideoReport> VideoReports { get; set; }

    public ApplicationDbContext(IConfiguration configuration, ICurrentUserService currentUserService,
        IDateTimeService dateTime)
    {
        _configuration = configuration;
        _currentUserService = currentUserService;
        _dateTime = dateTime;
    }

    public async Task BeginTransactionAsync()
        {
            if (_currentTransaction != null) return;

            _currentTransaction = await base.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted)
                .ConfigureAwait(false);
        }
    
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                await SaveChangesAsync(cancellationToken).ConfigureAwait(false);

                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    
    public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(
            _configuration.GetConnectionString("DefaultConnection"));
    }
    
    
    private void OnSavingChanges(object sender, SavingChangesEventArgs e)
    {
        _cleanString();
        ConfigureEntityDates();
    }

    private void _cleanString()
    {
        // var changedEntities = ChangeTracker.Entries()
        //     .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);
        // foreach (var item in changedEntities)
        // {
        //     if (item.Entity == null)
        //         continue;
        //
        //     var properties = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //         .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));
        //
        //     foreach (var property in properties)
        //     {
        //         var propName = property.Name;
        //         var val = (string)property.GetValue(item.Entity, null);
        //
        //         if (val.HasValue())
        //         {
        //             var newVal = val.Fa2En().FixPersianChars();
        //             if (newVal == val)
        //                 continue;
        //             property.SetValue(item.Entity, newVal, null);
        //         }
        //     }
        // }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);
        //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<Achievement>().ToTable("achievements");
        modelBuilder.Entity<AthleteDocument>().ToTable("athleteDocuments");
        modelBuilder.Entity<CoachDocument>().ToTable("coachDocuments");
        modelBuilder.Entity<ManagerDocument>().ToTable("managerDocuments");
        modelBuilder.Entity<MedicalStaffDocument>().ToTable("medicalStaffDocuments");
        modelBuilder.Entity<Message>().ToTable("messages");
        modelBuilder.Entity<Notification>().ToTable("notifications");
        modelBuilder.Entity<NutritionistDocument>().ToTable("nutritionistDocuments");
        modelBuilder.Entity<Rating>().ToTable("ratings");
        modelBuilder.Entity<Service>().ToTable("services");
        modelBuilder.Entity<Service>().ToTable("services");
        modelBuilder.Entity<Speciality>().ToTable("specialities");
        modelBuilder.Entity<TrainingOffer>().ToTable("trainingOffers");
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<Video>().ToTable("videos");
        modelBuilder.Entity<VideoReport>().ToTable("videoReports");

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var propertyInfo = entityType.ClrType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
            {
                // Apply the configuration for that property
                modelBuilder.Entity(entityType.Name).Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
            }
        }
        //var entitiesAssembly = typeof(IEntity).Assembly;
        //modelBuilder.RegisterAllEntities<IEntity>(entitiesAssembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        //modelBuilder.AddRestrictDeleteBehaviorConvention();
        //modelBuilder.AddPluralizingTableNameConvention();
        
    }

    private void ConfigureEntityDates()
    {
        var updatedEntities = ChangeTracker.Entries().Where(x =>
            x.Entity is ITimeModification && x.State == EntityState.Modified).Select(x => x.Entity as ITimeModification);

        var addedEntities = ChangeTracker.Entries().Where(x =>
            x.Entity is ITimeModification && x.State == EntityState.Added).Select(x => x.Entity as ITimeModification);

        foreach (var entity in updatedEntities)
        {
            if (entity != null)
            {
                entity.ModifiedDate = DateTime.Now;
            }
        }

        foreach (var entity in addedEntities)
        {
            if (entity != null)
            {
                entity.CreatedTime = DateTime.Now;
                entity.ModifiedDate = DateTime.Now;
            }
        }
    }

    
}