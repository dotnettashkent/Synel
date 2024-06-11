using Shared.Infrastructures;
using Shared.Infrastructures.Extensions;
using Stl.Async;
using Stl.CommandR.Configuration;
using Stl.Fusion;
using System.Reactive;

namespace Shared.Features.Employees
{
    public interface IEmployeeService : IComputeService
    {
        //[ComputeMethod]
        Task<TableResponse<EmployeeView>> GetAll(TableOptions options, CancellationToken cancellationToken = default);
        //[ComputeMethod]
        Task<EmployeeView> Get(long id, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Create(CreateEmployeeCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Update(UpdateEmployeeCommand command, CancellationToken cancellationToken = default);
        [CommandHandler]
        Task Delete(DeleteEmployeeCommand command, CancellationToken cancellationToken = default);
        Task<Unit> Invalidate() { return TaskExt.UnitTask; }
    }
}
