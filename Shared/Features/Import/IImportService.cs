using Stl.Fusion;

namespace Shared.Features.Import
{
    public interface IImportService 
    {
        Task ImportEmployeesFromExcel(Stream fileStream, CancellationToken cancellationToken);
    }
}
