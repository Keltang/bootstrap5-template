using System.Diagnostics;
using System.Reflection;
using BlackBox.Application.Common.Interfaces;
using BlackBox.Domain.Common;
using BlackBox.Domain.Entities;
using BlackBox.Domain.Exceptions; 
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlackBox.Infrastructure.Persistence.Contexts
{
    public class BlackBoxContext : IdentityDbContext, IBlackBoxContext
    {
        private readonly IDateTimeService _dateTimeService;
        private readonly ICurrentUserService _currentUserService; 

        public BlackBoxContext(DbContextOptions<BlackBoxContext> options, 
                        IDateTimeService dateTimeService, 
                        ICurrentUserService currentUserService) : base(options)
        {
            _dateTimeService = dateTimeService;
            _currentUserService = currentUserService; 
        }

        public async Task<int> ExecuteAction(string sqlStatement, CancellationToken cancellationToken = new CancellationToken())
        {
            return await Database.ExecuteSqlRawAsync(sqlStatement, cancellationToken);
        }

        public void UpdateEntity(object entity)
        {
            base.Update(entity);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
#if DEBUG
            optionsBuilder.LogTo(message => Debug.WriteLine(message))
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
#endif
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(builder);
        }

        // public override DatabaseFacade Database => base.Database;

        #region Database Entities  
        public DbSet<Grade> Grades => Set<Grade>(); 

        #endregion


        #region SaveChanges
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>())
            {
                //string _tempUserId = "DBD46B01-F080-43FB-BA0D-8DA192F420E7";
                if (string.IsNullOrWhiteSpace(_currentUserService.UserId))
                    throw new BusinessRuleException("UserId", "A user Id is required for auditable persistence operation");

                switch (entry.State)
                {
                    case EntityState.Added:
                        /*entry.Entity.CreatedBy = string.IsNullOrWhiteSpace(entry.Entity.CreatedBy)
                            ? _currentUserService.UserId
                            : entry.Entity.CreatedBy;*/
                        entry.Entity.CreatedBy = _currentUserService.UserId; // ?? _tempUserId;
                        entry.Entity.CreatedOn = _dateTimeService.EstNow;
                        if (entry.Entity.Id == default)
                            entry.Entity.GenerateId();
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _currentUserService.UserId; // ?? _tempUserId;
                        entry.Entity.LastModifiedOn = _dateTimeService.EstNow;
                        break;
                }
            }
             
            var result = await base.SaveChangesAsync(cancellationToken);
             
            return result;
        }

        public async Task<int> SaveChangesExplicitTrackingAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (string.IsNullOrWhiteSpace(entry.Entity.CreatedBy))
                            throw new BusinessRuleException("UserId", "A user Id is required for auditable persistence operation");

                        entry.Entity.CreatedOn = _dateTimeService.EstNow;
                        if (entry.Entity.Id == default)
                            entry.Entity.GenerateId();
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = entry.Entity.LastModifiedBy;
                        entry.Entity.LastModifiedOn = _dateTimeService.EstNow;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
         
        #endregion 

    }
}
