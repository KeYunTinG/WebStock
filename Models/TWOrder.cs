namespace WebStock.Models
{
    public class TWOrder
    {
        public int ID { get; set; }
        public int stockID { get; set; }
        public int Inventory { get; set; }
        public DateTime? BuyingTime { get; set; }
        public DateTime? SellingTime { get; set; }
    }
}
