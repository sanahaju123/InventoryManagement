using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InventoryManagement.App.Helper.ExportToCsv
{
    public class CsvExporter
    {
        public IActionResult ExportToCsv<T>(List<T> data, string fileName)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new StreamWriter(memoryStream))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data);
                }

                memoryStream.Position = 0;

                var contentDisposition = new InlineDisposition(fileName);
                return new FileStreamResult(memoryStream, "text/csv")
                {
                    FileDownloadName = contentDisposition.FileName,
                };
            }
        }

    }
    public class InlineDisposition
    {
        public string FileName { get; }

        public InlineDisposition(string fileName)
        {
            FileName = fileName;
        }
    }
}
