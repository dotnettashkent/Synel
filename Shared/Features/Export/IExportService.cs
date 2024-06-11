using Shared.Infrastructures;
using Stl.Fusion;

namespace Shared.Features.Export
{
    public interface IExportService : IComputeService
    {
        Task<Stream> ExportEmployeeToExcel(TableOptions tableOptions, CancellationToken cancellationToken);
    }
}
