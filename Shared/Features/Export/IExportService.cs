using Shared.Infrastructures;

namespace Shared.Features.Export
{
    public interface IExportService
    {
        Task<string> ExportSubscribersToExcel(TableOptions tableOptions, CancellationToken cancellationToken);
    }
}
