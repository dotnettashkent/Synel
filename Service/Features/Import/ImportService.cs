using ClosedXML.Excel;
using Microsoft.AspNetCore.Session;
using Shared.Features.Employees;
using Shared.Features.Import;
using Stl.CommandR;
using Stl.Fusion;

public class ImportService : IImportService
{
    private readonly ICommander _commander;

    public ImportService(ICommander commander)
    {
        _commander = commander;
    }

    public async Task ImportEmployeesFromExcel(Stream excelStream, CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook(excelStream);
        var worksheet = workbook.Worksheet(1);
        var rows = worksheet.RangeUsed().RowsUsed().Skip(1); // Skip header row

        foreach (var row in rows)
        {
            var employee = new CreateEmployeeCommand(Session.Default, new EmployeeView
            {
                PayrollNumber = row.Cell(1).GetString(),
                Forenames = row.Cell(2).GetString(),
                Surname = row.Cell(3).GetString(),
                DateOfBirth = row.Cell(4).GetDateTime().ToUniversalTime(),
                Telephone = row.Cell(5).GetString(),
                Mobile = row.Cell(6).GetString(),
                Address = row.Cell(7).GetString(),
                Address2 = row.Cell(8).GetString(),
                Postcode = row.Cell(9).GetString(),
                EmailHome = row.Cell(10).GetString(),
                StartDate = row.Cell(11).GetDateTime().ToUniversalTime()
            });

            // Call the command handler using ICommander.Call(...)
            await _commander.Call(employee, cancellationToken);
        }
    }
}
