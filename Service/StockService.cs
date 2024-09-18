using System.Data.SqlClient;
using Dapper;
using WebStock.Common.InterFace;
using WebStock.Models.Interface;

namespace WebStock.Service
{
    public class StockService(IConfiguration _configuration, IWebCrawler _webCrawler) : IStockService
    {
        decimal 股票價值總和 = 0;
        decimal 利息總和 = 0;
        public async Task<StockTWModel> 取得所有股票()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DB")))
            {
                string query = "SELECT tw.ID, tw.Code, tw.Name,twOrder.Inventory, twOrder.BuyingTime, twOrder.SellingTime  FROM TW tw INNER JOIN TWOrder twOrder ON tw.ID = twOrder.StockID";
                var stocks = await connection.QueryAsync<StockTWData>(query);
                var stockViewModels = new List<StockTW>();

                foreach (var stock in stocks)
                {
                    var stockPrice = await _webCrawler.爬取股價(stock.Code);

                    股票價值總和 += stock.Inventory * stockPrice;
                    //取得當年度
                    利息總和 += await 取得個別股票利息(stock.ID, 2024);

                    stockViewModels.Add(new StockTW
                    {
                        ID = stock.ID,
                        Code = stock.Code,
                        Name = stock.Name,
                        Inventory = stock.Inventory,
                        StockPrice = stockPrice,
                        Value = stock.Inventory * stockPrice,
                        Dividend = await 取得個別股票利息(stock.ID,2024),
                    });
                }

                return new StockTWModel
                {
                    Stocks = stockViewModels,
                    Total = 股票價值總和,
                    DividendTotal = 利息總和
                };
            }
        }
        public async Task<decimal> 取得個別股票利息(int StockID,int Year)
        {
            decimal 利息 = 0;
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DB")))
            {
                string query = "SELECT  o.ID, o.StockID,o.Inventory,o.BuyingTime,o.SellingTime,d.CashDividend,d.RecordDate,d.PaymentDay " +
                    "FROM TWOrder o " +
                    "LEFT JOIN TWStockDividendSchedule d ON o.StockID = d.StockID " +
                    "WHERE o.StockID = @StockID  " + 
                    "AND o.BuyingTime < d.RecordDate " +
                    "AND YEAR(d.RecordDate) = @Year";
                var parameters = new { StockID, Year };
                var stocks = await connection.QueryAsync<StockTWDividendData>(query, parameters);

                foreach (var stock in stocks)
                {
                    利息 += stock.Inventory * stock.CashDividend;
                }

                return 利息;
            }
        }
    }
}