using ClosedXML.Excel;
using Shared.Features.Employees;
using Shared.Features.Import;
using Stl.Fusion;

namespace Service.Features.Import
{
    public class ImportService : IImportService
    {
        private readonly IEmployeeService employeeService;
        private readonly Session session;

        public ImportService(IEmployeeService employeeService, Session session)
        {
            this.employeeService = employeeService;
            this.session = Session.Default;
        }

        public async Task ImportEmployeesFromExcel(Stream fileStream, CancellationToken cancellationToken)
        {
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheet("Employees");
            var rows = worksheet.RowsUsed().Skip(1); // Skipping header row

            var employees = new List<CreateEmployeeCommand>();

            foreach (var row in rows)
            {
                var employee = new CreateEmployeeCommand(session, new EmployeeView
                {
                    PayrollNumber = row.Cell(1).GetValue<string>(),
                    Forenames = row.Cell(2).GetValue<string>(),
                    Surname = row.Cell(3).GetValue<string>(),
                    DateOfBirth = row.Cell(4).GetValue<DateTime>(),
                    Telephone = row.Cell(5).GetValue<string>(),
                    Mobile = row.Cell(6).GetValue<string>(),
                    Address = row.Cell(7).GetValue<string>(),
                    Address2 = row.Cell(8).GetValue<string>(),
                    Postcode = row.Cell(9).GetValue<string>(),
                    EmailHome = row.Cell(10).GetValue<string>(),
                    StartDate = row.Cell(11).GetValue<DateTime>()
                });
                employees.Add(employee);
            }

            foreach (var employee in employees)
            {
                await employeeService.Create(employee, cancellationToken);
            }
        }
    }
}
