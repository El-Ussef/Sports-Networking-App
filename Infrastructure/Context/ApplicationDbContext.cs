using System.Data;
using System.Reflection;
using Application.Contracts;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Infrastructure.Context;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole,string>,IApplicationDbContext
{
    private readonly IConfiguration _configuration;
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTimeService _dateTime;
    private IDbContextTransaction _currentTransaction;
    
    // public DbSet<Athlete> Athletes { get; set; }
    // public DbSet<Coach> Coaches { get; set; }
    // public DbSet<Manager> Managers { get; set; }
    // public DbSet<Club> Clubs { get; set; }
    // public DbSet<Nutritionist> Nutritionists { get; set; }
    // public DbSet<MedicalStaff> MedicalStaves { get; set; }
    // public DbSet<Sponsor> Sponsors { get; set; }

    public DbSet<Achievement> Achievements { get; set; }
    public DbSet<AthleteDocument> AthleteDocuments { get; set; }
    public DbSet<CoachDocument> CoachDocuments { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Region> Regions { get; set; }
    public DbSet<Province> Provinces { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Location> Locations { get; set; }

    public DbSet<Event> Events { get; set; }
    public DbSet<ManagerDocument> ManagerDocuments { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<ServiceType> ServiceTypes { get; set; }
    public DbSet<ServiceExample> ServiceExamples { get; set; }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Speciality> Specialities { get; set; }
    public DbSet<Sport> Sports { get; set; }
    public DbSet<SportCategory> SportCategories { get; set; }
    public DbSet<TrainingOffer> TrainingOffers { get; set; }
    public DbSet<TrainingSession> TrainingSessions { get; set; }
    public DbSet<Video> Videos { get; set; }
    public DbSet<VideoReport> VideoReports { get; set; }
    
    public DbSet<ServiceTypeTranslation> ServiceTypeTranslations { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }


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
    
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var userId = $"{_currentUserService.UserName}:{_currentUserService.UserId}";

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    //entry.Entity.CreatedBy = userId;
                    entry.Entity.CreatedTime = _dateTime.Now.ToUniversalTime();
                    break;
                case EntityState.Modified:
                    //entry.Entity.LastModifiedBy = userId;
                   entry.Entity.ModifiedDate = _dateTime.Now.ToUniversalTime();
                    break;
            }

        return base.SaveChangesAsync(cancellationToken);
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

        //modelBuilder.ApplyUtcDateTimeConverter();
        base.OnModelCreating(modelBuilder);
        //builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        
        modelBuilder.Entity<Event>(ConfigureEvent);
        
        modelBuilder.HasPostgresEnum<UserType>();
        
        modelBuilder.Entity<ApplicationUser>()
            .Property(e=>e.UserType)
            .HasConversion<string>();
        
        // modelBuilder.Entity<ApplicationUser>()
        //     .HasDiscriminator(c=>c.UserType);
        // Configuring Message-Sender relationship
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuring Message-Receiver relationship
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany()
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserConnection>().HasKey(uc => uc.Id);
        
        modelBuilder.Entity<AppUser>()
            .HasDiscriminator<UserType>("UserType")
            .HasValue<AppUser>(UserType.BaseType)
            .HasValue<Athlete>(UserType.Athlete)
            .HasValue<Coach>(UserType.Coach)
            .HasValue<Club>(UserType.Club)
            .HasValue<Manager>(UserType.Manager)
            .HasValue<MedicalStaff>(UserType.MedicalAndHealth);

        // modelBuilder
        //     .Entity<AppUser>()
        //     .Property(e => e.UserType)
        //     .HasConversion(
        //         v => v.ToString(), // Convert Enum to String for the database
        //         v => (UserType)Enum.Parse(typeof(UserType), v) // Convert String back to Enum for the application
        //     );
        
        // modelBuilder.Entity<Athlete>().ToTable("athletes");
        // modelBuilder.Entity<Coach>().ToTable("coaches");
        // modelBuilder.Entity<Manager>().ToTable("managers");
        // modelBuilder.Entity<Club>().ToTable("clubs");
        // modelBuilder.Entity<Nutritionist>().ToTable("nutritionists");
        // modelBuilder.Entity<MedicalStaff>().ToTable("medicalStaffs");
        // modelBuilder.Entity<Sponsor>().ToTable("sponsors");
        modelBuilder.Entity<Location>()
            .OwnsOne(l => l.OpeningHours, oh =>
            {
                oh.Property(o => o.Start).HasColumnName("OpeningHoursStart");
                oh.Property(o => o.End).HasColumnName("OpeningHoursEnd");
            });

        modelBuilder.Entity<Location>()
            .Property(l => l.LocationImages)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        modelBuilder.Entity<Achievement>().ToTable("achievements");
        modelBuilder.Entity<AthleteDocument>().ToTable("athleteDocuments");
        modelBuilder.Entity<CoachDocument>().ToTable("coachDocuments");
        modelBuilder.Entity<ManagerDocument>().ToTable("managerDocuments");
        modelBuilder.Entity<Message>().ToTable("messages");
        modelBuilder.Entity<Notification>().ToTable("notifications");
        modelBuilder.Entity<Rating>().ToTable("ratings");
        modelBuilder.Entity<Service>().ToTable("services");
        modelBuilder.Entity<Sport>().ToTable("sports");
        modelBuilder.Entity<SportCategory>().ToTable("sportCategory");
        modelBuilder.Entity<Speciality>().ToTable("specialities");
        modelBuilder.Entity<TrainingOffer>().ToTable("trainingOffers"); 
        modelBuilder.Entity<Video>().ToTable("videos");
        modelBuilder.Entity<VideoReport>().ToTable("videoReports");
        modelBuilder.Entity<City>().ToTable("cities");
        modelBuilder.Entity<Region>().ToTable("regions");
        modelBuilder.Entity<Province>().ToTable("provinces");
        modelBuilder.Entity<ServiceType>().ToTable("serviceTypes");
        modelBuilder.Entity<Event>()
            .Property(e => e.EventImages)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
        
        modelBuilder.Entity<ServiceTypeTranslation>().ToTable("serviceTypeTranslations");
        modelBuilder.UseIdentityByDefaultColumns();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var propertyInfo = entityType.ClrType.GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null && propertyInfo.PropertyType == typeof(int))
            {
                // Apply the configuration for that property
                // modelBuilder.Entity(entityType.Name).Property<int>("Id")
                //     .ValueGeneratedOnAdd()
                //     .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);
                modelBuilder.Entity(entityType.Name).Property<int>("Id").UseIdentityAlwaysColumn();
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
    
    private void ConfigureEvent(EntityTypeBuilder<Event> builder)
    {
        // Value converter for the EventImages collection
        var converter = new ValueConverter<List<string>, string>(
            v => string.Join(',', v),
            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());

        // Value comparer for the EventImages collection
        var comparer = new ValueComparer<List<string>>(
            (c1, c2) => c1.SequenceEqual(c2),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList());

        builder.Property(e => e.EventImages)
            .HasConversion(converter)
            .Metadata.SetValueComparer(comparer);
    }

    
}