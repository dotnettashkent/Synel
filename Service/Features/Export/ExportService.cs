using ClosedXML.Excel;
using Shared.Features.Employees;
using Shared.Features.Export;
using Shared.Infrastructures;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Features.Export
{
    public class ExportService : IExportService
    {
        private readonly IEmployeeService employeeService;

        public ExportService(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        public async Task<Stream> ExportEmployeeToExcel(TableOptions tableOptions, CancellationToken cancellationToken)
        {
            var employees = await employeeService.GetAll(tableOptions, cancellationToken);
            using var package = new XLWorkbook();
            var worksheet = package.Worksheets.Add("Employees");
            var currentRow = 1;

            worksheet.Cell("A1").Value = "PayrollNumber";
            worksheet.Cell("B1").Value = "Forenames";
            worksheet.Cell("C1").Value = "Surname";
            worksheet.Cell("D1").Value = "DateOfBirth";
            worksheet.Cell("E1").Value = "Telephone";
            worksheet.Cell("F1").Value = "Mobile";
            worksheet.Cell("G1").Value = "Address";
            worksheet.Cell("H1").Value = "Address2";
            worksheet.Cell("I1").Value = "Postcode";
            worksheet.Cell("J1").Value = "EmailHome";
            worksheet.Cell("K1").Value = "StartDate";

            // Populate data
            int row = 2;
            foreach (var employee in employees.Items)
            {
                worksheet.Cell(row, 1).Value = employee.PayrollNumber;
                worksheet.Cell(row, 2).Value = employee.Forenames;
                worksheet.Cell(row, 3).Value = employee.Surname;
                worksheet.Cell(row, 4).Value = employee.DateOfBirth;
                worksheet.Cell(row, 5).Value = employee.Telephone;
                worksheet.Cell(row, 6).Value = employee.Mobile;
                worksheet.Cell(row, 7).Value = employee.Address;
                worksheet.Cell(row, 8).Value = employee.Address2;
                worksheet.Cell(row, 9).Value = employee.Postcode;
                worksheet.Cell(row, 10).Value = employee.EmailHome;
                worksheet.Cell(row, 11).Value = employee.StartDate;

                row++;
            }

            var memoryStream = new MemoryStream();
            package.SaveAs(memoryStream);
            memoryStream.Position = 0; // Reset the position of the stream to the beginning
            return memoryStream;
        }
    }
}
