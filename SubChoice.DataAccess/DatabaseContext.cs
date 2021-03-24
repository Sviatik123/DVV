using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using SubChoice.Core.Configuration;
using SubChoice.Core.Data.Entities;
using SubChoice.Core.Interfaces.DataAccess.Base;
using SubChoice.DataAccess.Seeders;

namespace SubChoice.DataAccess
{
    public class DatabaseContext : IdentityDbContext<User, Role, Guid>, IUnitOfWork
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public new DbSet<User> Users { get; set; }
        public new DbSet<Role> Roles { get; set; }

        public new EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class
        {
            return base.Entry(entity);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        public override int SaveChanges()
        {
            var entities = ChangeTracker.Entries().Where(x => x.Entity is ISaveTrackable &&
                            (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.Entity is ISaveTrackable entitySaveTrackable)
                {
                    var timeNow = DateTime.UtcNow;
                    if (entity.State == EntityState.Added)
                    {
                        entitySaveTrackable.CreatedOn = timeNow;
                        entitySaveTrackable.CreatedBy = UserId;
                    }

                    entitySaveTrackable.ModifiedOn = timeNow;
                    entitySaveTrackable.ModifiedBy = UserId;
                }
            }

            // resolving optimistic concurrency exceptions (client wins)
            // https://msdn.microsoft.com/en-in/data/jj592904.aspx
            var conflict = false;
            var retryCount = Constants.DbConcurrencyResolveRetryLimit;
            var res = 0;
            do
            {
                try
                {
                    conflict = false;

                    res = base.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    conflict = --retryCount > 0;
                    if (conflict)
                    {
                        // Update original values from the database (client wins)
                        var entry = ex.Entries.Single();
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    }
                    else
                    {
                        throw; // give up
                    }
                }
            } while (conflict);

            return res;
        }

        private Guid UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null)
                {
                    if (Guid.TryParse(userIdClaim.Value, out var userId))
                    {
                        return userId;
                    }
                }

                return Guid.Empty;
            }
        }

        public async Task CreateOrMigrateAsync(bool clean = false)
        {
            if (clean)
            {
                await Database.EnsureDeletedAsync();
            }
            else
            {
                await Database.MigrateAsync();
            }
        }

        public IDbContextTransaction BeginTransaction()
        {
            return Database.BeginTransaction();
        }

        public void Commit(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }

        public void Rollback(IDbContextTransaction transaction)
        {
            transaction.Rollback();
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            RolesSeeder.SeedData(modelBuilder);
        }
    }
}
