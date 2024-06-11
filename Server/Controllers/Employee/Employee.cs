using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.Features.Employees;
using Shared.Infrastructures.Extensions;
using Shared.Infrastructures;
using Stl.CommandR;
using System.IO.Packaging;
using Shared.Features.Export;
using Service.Features.Export;

namespace Server.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class Employee : ControllerBase
    {
        private readonly IEmployeeService employeeService;

        private readonly ICommander commander;
        private readonly IExportService exportService;
        public Employee(IEmployeeService employeeService, ICommander commander, IExportService exportService)
        {
            this.employeeService = employeeService;
            this.commander = commander;
            this.exportService = exportService;
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
        public async Task<IActionResult> ExportSubscribersToExcel([FromQuery] TableOptions tableOptions, CancellationToken cancellationToken = default)
        {
            var stream = await exportService.ExportEmployeeToExcel(tableOptions, cancellationToken);

            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Employees.xlsx";
            return File(stream, contentType, fileName);
        }
    }
}
