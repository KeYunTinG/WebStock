using System.Data.SqlClient;
using Dapper;
using WebStock.Common.InterFace;
using WebStock.Models;
using WebStock.Models.Interface;

namespace WebStock.Service
{
    public class StockService : IStockService
    {
        private readonly string? _conn;
        private readonly IWebCrawler _webCrawler;
        public StockService(IConfiguration configuration, IWebCrawler WebCrawler)
        {
            _conn = configuration.GetConnectionString("DB");
            _webCrawler = WebCrawler;
        }
        public async Task<StockTWModel> GetStock()
        {
            using (var connection = new SqlConnection(_conn))
            {
                string query = "SELECT tw.ID, tw.Code, tw.Name,twOrder.Inventory, twOrder.BuyingTime, twOrder.SellingTime  FROM TW tw INNER JOIN TWOrder twOrder ON tw.ID = twOrder.StockID";
                var stocks = await connection.QueryAsync<StockTWData>(query);
                var stockViewModels = new List<StockTW>();
                decimal total = 0;
                decimal dividendTotal = 0;

                foreach (var stock in stocks)
                {
                    var stockPrice = await _webCrawler.GetStockPrice(stock.Code);

                    total += stock.Inventory * stockPrice;
                    dividendTotal += await GetDividend(stock.ID, 2024);

                    stockViewModels.Add(new StockTW
                    {
                        ID = stock.ID,
                        Code = stock.Code,
                        Name = stock.Name,
                        Inventory = stock.Inventory,
                        StockPrice = stockPrice,
                        Value = stock.Inventory * stockPrice,
                        Dividend = await GetDividend(stock.ID,2024),
                    });
                }

                return new StockTWModel
                {
                    Stocks = stockViewModels,
                    Total = total,
                    DividendTotal = dividendTotal
                };
            }
        }
        public async Task<decimal> GetDividend(int StockID,int Year)
        {
            using (var connection = new SqlConnection(_conn))
            {
                string query = "SELECT  o.ID, o.StockID,o.Inventory,o.BuyingTime,o.SellingTime,d.CashDividend,d.RecordDate,d.PaymentDay " +
                    "FROM TWOrder o " +
                    "LEFT JOIN TWStockDividendSchedule d ON o.StockID = d.StockID " +
                    "WHERE o.BuyingTime < d.RecordDate  " +
                    "AND o.StockID = @StockID " +
                    "AND YEAR(d.RecordDate) = @Year";
                var parameters = new { StockID, Year };
                var stocks = await connection.QueryAsync<StockTWDividendData>(query, parameters);
                decimal total = 0;

                foreach (var stock in stocks)
                {
                    total += stock.Inventory * stock.CashDividend;
                }

                return total;
            }
        }
    }
}