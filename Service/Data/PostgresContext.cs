using Microsoft.EntityFrameworkCore;
using Shared.Features.Employees;

namespace Service.Data
{
    public partial class AppDbContext
    {
        public virtual DbSet<EmployeeEntity> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional configuration for Employees if needed
        }
    }
}
