using BusesControl.Export.Core.Enums;

namespace BusesControl.Export.Core.Entities
{
    public class FinancialModel
    {
        public string Reference { get; set; }
        public FinancialTypeEnum Type { get; set; }
        public string CustomerName { get; set; }
        public string SupplierName { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPaid { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public DateTime TerminateDate { get; set; }
        public bool Active { get; set; }
    }
}
