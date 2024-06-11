using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features.Employees;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;
using Shared.Features.Export;
using Shared.Features.Import;
using NSwag.Annotations;

namespace Server.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employee : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        private readonly ICommander commander;
        private readonly IExportService exportService;
        private readonly IImportService importService;
        public Employee(IEmployeeService employeeService, ICommander commander, IExportService exportService, IImportService importService)
        {
            this.employeeService = employeeService;
            this.commander = commander;
            this.exportService = exportService;
            this.importService = importService;
        }

        [HttpPost]
        public async Task<EmployeeView> Create([FromBody] CreateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            return await commander.Call(command, cancellationToken);
        }

        [HttpDelete]
        public async Task Delete([FromBody] DeleteEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpPut]
        public async Task Update([FromBody] UpdateEmployeeCommand command, CancellationToken cancellationToken = default)
        {
            await commander.Call(command, cancellationToken);
        }

        [HttpGet("")]
        public async Task<TableResponse<EmployeeView>> GetAll([FromQuery] TableOptions options, CancellationToken cancellationToken = default)
        {
            return await employeeService.GetAll(options, cancellationToken);
        }

        [HttpGet("{Id}")]
        public Task<EmployeeView> Get(long Id, CancellationToken cancellationToken)
        {
            return employeeService.Get(Id, cancellationToken);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportEmployeesToExcel([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken = default)
        {
            var stream = await exportService.ExportEmployeeToExcel(tableOptions, cancellationToken);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Employees.xlsx";
            return File(stream, contentType, fileName);
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportEmployeeFromExcel([OpenApiFile] IFormFile file, CancellationToken cancellationToken)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is empty.");

            using var stream = new MemoryStream();
            await file.CopyToAsync(stream, cancellationToken);
            stream.Position = 0;

            await importService.ImportEmployeesFromExcel(stream, cancellationToken);

            return Ok("Employees imported successfully.");
        }
    }
}
