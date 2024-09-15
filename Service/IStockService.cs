using WebStock.Models.Interface;

namespace WebStock.Service
{
    public interface IStockService
    {
        Task<StockTWModel> GetStock();
        Task<decimal> GetDividend(int StockID, int Year);
    }
}
