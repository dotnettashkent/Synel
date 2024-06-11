using Microsoft.EntityFrameworkCore;
using Service.Data;
using Shared.Features.Employees;
using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.Fusion;
using Stl.Fusion.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Reactive;
#pragma warning disable

namespace Service.Features.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly DbHub<AppDbContext> dbHub;

        public EmployeeService(DbHub<AppDbContext> dbHub)
        {
            this.dbHub = dbHub;
        }

        public async virtual Task<TableResponse<EmployeeView>> GetAll(TableOptions options, CancellationToken cancellationToken = default)
        {
            await Invalidate();
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var employees = from s in dbContext.Employees select s;

            // this is using for Search operation
            if (!String.IsNullOrEmpty(options.Search))
            {
                employees = employees.Where(s =>
                         s.PayrollNumber.Contains(options.Search)
                         || s.Postcode.Contains(options.Search)
                         || s.Surname.Contains(options.Search)
                         || s.Telephone.Contains(options.Search)
                         || s.Mobile.Contains(options.Search)
                         || s.Forenames.Contains(options.Search)
                         || s.EmailHome.Contains(options.Search)
                         || s.Address.Contains(options.Search)
                         || s.Address2.Contains(options.Search)
                );
            }

            Sorting(ref employees, options);

            var count = await employees.AsNoTracking().CountAsync(cancellationToken: cancellationToken);
            var items = await employees.AsNoTracking().Paginate(options).ToListAsync(cancellationToken: cancellationToken);
            return new TableResponse<EmployeeView>() { Items = items.MapToViewList(), TotalItems = count };
        }

        public async virtual Task<EmployeeView> Get(long Id, CancellationToken cancellationToken = default)
        {
            var dbContext = dbHub.CreateDbContext();
            await using var _ = dbContext.ConfigureAwait(false);
            var address = await dbContext.Employees
            .FirstOrDefaultAsync(x => x.Id == Id);

            return address == null ? throw new ValidationException("EmployeeEntity Not Found") : address.MapToView();
        }

        public async virtual Task Create(CreateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }

            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            EmployeeEntity employee = new EmployeeEntity();
            Reattach(employee, command.Entity, dbContext);

            dbContext.Update(employee);
            await dbContext.SaveChangesAsync();
        }

        public async virtual Task Delete(DeleteEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var employee = await dbContext.Employees
                .FirstOrDefaultAsync(x => x.Id == command.Id);

            if (employee == null) throw new ValidationException("EmployeeEntity Not Found");
            dbContext.Remove(employee);
            await dbContext.SaveChangesAsync();
        }



        public async virtual Task Update(UpdateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            if (Computed.IsInvalidating())
            {
                _ = await Invalidate();
                return;
            }
            await using var dbContext = await dbHub.CreateCommandDbContext(cancellationToken);
            var employee = await dbContext.Employees
                .FirstOrDefaultAsync(x => x.Id == command.Entity!.Id);

            if (employee == null) throw new ValidationException("EmployeeEntity Not Found");

            Reattach(employee, command.Entity, dbContext);

            await dbContext.SaveChangesAsync();
        }

        #region Helper
        public virtual Task<Unit> Invalidate() => TaskExt.UnitTask;
        private void Reattach(EmployeeEntity entity, EmployeeView view, AppDbContext dbContext)
        {
            EmployeeMapper.From(view, entity);
        }

        /// <summary>
        /// this function using Sorting all items
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="options"></param>
        /// 

        private void Sorting(ref IQueryable<EmployeeEntity> unit, TableOptions options) => unit = options.SortLabel switch
        {
            "Id" => unit.Ordering(options, o => o.Id),
            "Surname" => unit.Ordering(options, o => o.Surname),
            "Forenames" => unit.Ordering(options, o => o.Forenames),
            "Mobile" => unit.Ordering(options, o => o.Mobile),
            "PayrollNumber" => unit.Ordering(options, o => o.PayrollNumber),
            "Postcode" => unit.Ordering(options, o => o.Postcode),
            "Address" => unit.Ordering(options, o => o.Address),
            "Address2" => unit.Ordering(options, o => o.Address2),
            "Telephone" => unit.Ordering(options, o => o.Telephone),
            "EmailHome" => unit.Ordering(options, o => o.EmailHome),
            _ => unit.OrderBy(o => o.Id),
        };
        #endregion
    }
}
