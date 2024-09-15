namespace WebStock.Models.Interface
{
    public class StockTWData
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Inventory { get; set; }
        public DateTime? BuyingTime { get; set; }
        public DateTime? SellingTime { get; set; }
    }
    public class StockTWDividendData
    {
        public int ID { get; set; }
        public int stockID { get; set; }
        public int Inventory { get; set; }
        public DateTime? BuyingTime { get; set; }
        public DateTime? SellingTime { get; set; }
        public decimal CashDividend { get; set; }
        public DateTime? RecordDate { get; set; }
        public DateTime? PaymentDay { get; set; }
    }
    public class StockTW
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Inventory { get; set; }
        public decimal StockPrice { get; set; }
        public decimal Value { get; set; }
        public decimal Dividend { get; set; }

    }
    public class StockTWModel
    {
        public IEnumerable<StockTW> Stocks { get; set; }
        public decimal Total { get; set; }
        public decimal DividendTotal { get; set; }
    }
}
 