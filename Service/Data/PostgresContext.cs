using Microsoft.EntityFrameworkCore;
using Shared.Features.Employees;

namespace Service.Data
{
    public partial class AppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public virtual DbSet<EmployeeEntity> Employees { get; set; }
        
    }

}
