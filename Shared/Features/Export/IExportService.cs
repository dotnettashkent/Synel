using Shared.Infrastructures;

namespace Shared.Features.Export
{
    public interface IExportService
    {
        Task<Stream> ExportEmployeeToExcel(TableOptions tableOptions, CancellationToken cancellationToken);
    }
}
