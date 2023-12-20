namespace LokataAdministrative2.Models.Citation
{
    public class PaymentSummaryDto
    {
        public StorageRateDto? StorageRate { get; set; }
        public TowingRateDto? TowingRate { get; set; }
        public double TotalViolationFees { get; set; }
        public DateTime Date { get; set; }
    }
}
