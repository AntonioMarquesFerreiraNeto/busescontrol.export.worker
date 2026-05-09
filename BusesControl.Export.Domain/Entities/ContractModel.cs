using BusesControl.Export.Core.Enums;

namespace BusesControl.Export.Core.Entities
{
    public class ContractModel
    {
        public Guid Id { get; set; }
        public string Reference { get; set; }
        public Guid BusId { get; set; }
        public string BusName { get; set; }
        public string LicensePlate { get; set; }
        public Guid DriverId { get; set; }
        public string DriverName { get; set; }
        public decimal TotalPrice { get; set; }
        public PaymentTypeEnum PaymentType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime TerminateDate { get; set; }
        public bool IsApproved { get; set; } = false;
        public Guid? ApproverId { get; set; }
        public string ApproverName { get; set; }
        public int CustomersCount { get; set; }
        public ContractStatusEnum Status { get; set; }
    }
}
