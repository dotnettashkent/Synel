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
            var employee = await employeeService.GetAll(tableOptions, cancellationToken);
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

            int totalRows = employee.Items.Count();
            for (int i = 1; i <= totalRows; i++)
            {
                worksheet.Cell(currentRow + i, 1);
                worksheet.Cell(currentRow + i, 2);
                worksheet.Cell(currentRow + i, 3);
                worksheet.Cell(currentRow + i, 4);
                worksheet.Cell(currentRow + i, 5);
                worksheet.Cell(currentRow + i, 6);
                worksheet.Cell(currentRow + i, 7);
                worksheet.Cell(currentRow + i, 8);
                worksheet.Cell(currentRow + i, 9);
                worksheet.Cell(currentRow + i, 10);
                worksheet.Cell(currentRow + i, 11);
            }

            // Populate data
            int row = 2;
            foreach (var item in employee.Items)
            {
                worksheet.Cell(row, 1).Value = item.PayrollNumber;
                worksheet.Cell(row, 2).Value = item.Forenames;
                worksheet.Cell(row, 3).Value = item.Surname;
                worksheet.Cell(row, 4).Value = item.DateOfBirth;
                worksheet.Cell(row, 5).Value = item.Telephone;
                worksheet.Cell(row, 6).Value = item.Mobile;
                worksheet.Cell(row, 7).Value = item.Address;
                worksheet.Cell(row, 8).Value = item.Address2;
                worksheet.Cell(row, 9).Value = item.Postcode;
                worksheet.Cell(row, 10).Value = item.EmailHome;
                worksheet.Cell(row, 11).Value = item.StartDate;

                row++;
            }

            var memoryStream = new MemoryStream();
            package.SaveAs(memoryStream);
            memoryStream.Position = 0; // Reset the position of the stream to the beginning
            return memoryStream;
        }
    }
}
