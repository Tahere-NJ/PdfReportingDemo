using PdfReportingDemo.Models;
using PdfReportingDemo.Services;

namespace PdfReportingDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("PDF Reporting Demo");

            // Initialize services
            var templateService = new TemplateService();
            var pdfService = new PdfService();

            // Create sample invoice data
            var invoice = new InvoiceModel
            {
                InvoiceNumber = "INV-2024-001",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                CustomerName = "John Doe",
                CustomerAddress = "123 Main St\nAnytown, ST 12345",
                Items = new List<InvoiceItem>
                {
                    new InvoiceItem { Description = "Web Development", Quantity = 40, UnitPrice = 75.00m },
                    new InvoiceItem { Description = "Database Design", Quantity = 20, UnitPrice = 85.00m },
                    new InvoiceItem { Description = "Testing & QA", Quantity = 15, UnitPrice = 65.00m }
                }
            };

            try
            {
                // Generate invoice PDF
                Console.WriteLine("Generating invoice PDF...");
                var invoiceHtml = templateService.GenerateInvoiceHtml(invoice);
                var invoicePdfPath = "invoice.pdf";
                pdfService.GeneratePdf(invoiceHtml, invoicePdfPath);
                Console.WriteLine($"Invoice PDF generated: {invoicePdfPath}");

                // Generate report PDF
                Console.WriteLine("Generating report PDF...");
                var reportData = new
                {
                    Title = "Monthly Sales Report",
                    GeneratedDate = DateTime.Now.ToString("MMMM dd, yyyy"),
                    TotalInvoices = 25,
                    TotalRevenue = 45750.00m,
                    TopCustomer = "ABC Corporation"
                };

                var reportHtml = templateService.GenerateReportHtml(reportData);
                var reportPdfPath = "report.pdf";
                pdfService.GeneratePdf(reportHtml, reportPdfPath);
                Console.WriteLine($"Report PDF generated: {reportPdfPath}");

                Console.WriteLine("PDF generation completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}