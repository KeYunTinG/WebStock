using WebStock.Models.Interface;

namespace WebStock.Service
{
    public interface IStockService
    {
        Task<StockTWModel> 取得所有股票();
        Task<decimal> 取得個別股票利息(int StockID, int Year);
    }
}
