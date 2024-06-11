using Service.Features.Employees;
using Service.Features.Export;
using Shared.Features.Employees;
using Shared.Features.Export;
using Shared.Features.Import;
using Stl.Fusion;

namespace Server
{
    public static class FusionServerExtension
    {
        public static FusionBuilder AddEmployeeServices(this FusionBuilder fusion)
        {
            fusion.AddService<IEmployeeService, EmployeeService>();
            fusion.AddService<IExportService, ExportService>();
            return fusion;
        }
        
    }
}
