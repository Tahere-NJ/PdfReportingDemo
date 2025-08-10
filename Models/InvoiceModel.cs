namespace PdfReportingDemo.Models
{
    public class InvoiceModel
    {
        public required string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public required string CustomerName { get; set; }
        public required string CustomerAddress { get; set; }
        public List<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();

        public decimal Subtotal => Items.Sum(item => item.Total);
        public decimal TaxRate => 0.08m; // 8% tax
        public decimal TaxAmount => Subtotal * TaxRate;
        public decimal Total => Subtotal + TaxAmount;
    }

    public class InvoiceItem
    {
        public string? Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Total => Quantity * UnitPrice;
    }
}