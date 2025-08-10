using HandlebarsDotNet;
using PdfReportingDemo.Models;

namespace PdfReportingDemo.Services
{
    public class TemplateService
    {
        private readonly IHandlebars _handlebars;

        public TemplateService()
        {
            _handlebars = Handlebars.Create();
            RegisterPartials();
            RegisterHelpers();
        }

        private void RegisterPartials()
        {
            // Register header partial
            var headerTemplate = GetTemplate("Templates/Partials/header.hbs");
            _handlebars.RegisterTemplate("header", headerTemplate);

            // Register footer partial
            var footerTemplate = GetTemplate("Templates/Partials/footer.hbs");
            _handlebars.RegisterTemplate("footer", footerTemplate);
        }

        private void RegisterHelpers()
        {
            // Currency formatting helper
            _handlebars.RegisterHelper("currency", (writer, context, parameters) =>
            {
                if (parameters.Length > 0 && decimal.TryParse(parameters[0].ToString(), out decimal amount))
                {
                    writer.WriteSafeString(amount.ToString("C"));
                }
            });

            // Date formatting helper
            _handlebars.RegisterHelper("formatDate", (writer, context, parameters) =>
            {
                if (parameters.Length > 0 && DateTime.TryParse(parameters[0].ToString(), out DateTime date))
                {
                    writer.WriteSafeString(date.ToString("MMMM dd, yyyy"));
                }
            });
        }

        public string GenerateInvoiceHtml(InvoiceModel invoice)
        {
            var template = GetTemplate("Templates/invoice.hbs");
            var compiledTemplate = _handlebars.Compile(template);
            return compiledTemplate(invoice);
        }

        public string GenerateReportHtml(object reportData)
        {
            var template = GetTemplate("Templates/report.hbs");
            var compiledTemplate = _handlebars.Compile(template);
            return compiledTemplate(reportData);
        }

        private string GetTemplate(string templatePath)
        {
            if (!File.Exists(templatePath))
            {
                throw new FileNotFoundException($"Template file not found: {templatePath}");
            }

            return File.ReadAllText(templatePath);
        }
    }
}