namespace WebStock.Models
{
    public class TWStockDividendSchedule
    {
        public int ID { get; set; }
        public int stockID { get; set; }
        public decimal CashDividend { get; set; }
        public DateTime? RecordDate { get; set; }
        public DateTime? PaymentDay { get; set; }

    }
}
