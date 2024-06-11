using Stl.Fusion;

namespace Shared.Features.Import
{
    public interface IImportService : IComputeService
    {
        Task ImportEmployeesFromExcel(Stream fileStream, CancellationToken cancellationToken);
    }
}
