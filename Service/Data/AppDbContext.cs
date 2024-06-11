using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stl.Fusion.Authentication.Services;
using Stl.Fusion.EntityFramework.Operations;
using Stl.Fusion.EntityFramework;
using Stl.Fusion.Extensions.Services;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Data
{
    public partial class AppDbContext : DbContextBase
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        [ActivatorUtilitiesConstructor]
        public AppDbContext(DbContextOptions<AppDbContext> options,
            IServiceScopeFactory serviceScopeFactory) : base(options)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        // Stl.Fusion.EntityFramework tables
        public DbSet<DbUser<string>> StlFusionUsers { get; protected set; } = null!;
        public DbSet<DbUserIdentity<string>> UserIdentities { get; protected set; } = null!;
        public DbSet<DbSessionInfo<string>> Sessions { get; protected set; } = null!;
        public DbSet<DbKeyValue> KeyValues { get; protected set; } = null!;
        public DbSet<DbOperation> Operations { get; protected set; } = null!;

        public override int SaveChanges()
        {
            AddTimestamps();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddTimestamps();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void AddTimestamps()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is IHasTimestamps && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var now = DateTime.UtcNow;
                if (entity.State == EntityState.Added)
                {
                    ((IHasTimestamps)entity.Entity).CreatedAt = now;
                }

                ((IHasTimestamps)entity.Entity).UpdatedAt = now;
            }
        }
    }

    public interface IHasTimestamps
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
