using AngleSharp;
using AngleSharp.Io;
using WebStock.Common.InterFace;

namespace WebStock.Common
{
    public class WebCrawler : IWebCrawler
    {
        public async Task<decimal> GetStockPrice(string stockID)
        {
            var url = $"https://tw.stock.yahoo.com/quote/{stockID}";

            var config = Configuration.Default.WithDefaultLoader(new LoaderOptions
            {
                IsResourceLoadingEnabled = true
            });

            var browser = BrowsingContext.New(config);
            decimal price = 0;
            try
            {
                var document = await browser.OpenAsync(url);

                var tables = document?.QuerySelectorAll("li.price-detail-item");

                if (tables != null && tables.Any())
                {
                    var stockData = tables.ToDictionary(
                        t => t.FirstElementChild?.TextContent.Trim(), // 欄位名稱 ex: 開盤價
                        t => t.LastElementChild?.TextContent.Trim()  // 欄位數值 ex: 500
                    );

                    // 將結果顯示在 label1 上
                    //label1.Text = string.Join(Environment.NewLine, stockData.Select(kv => $"{kv.Key}: {kv.Value}"));
                    var firstItem = stockData.FirstOrDefault();
                    price = decimal.Parse(firstItem.Value);
                }
                else
                {
                    //無法取得資料
                    return -1;
                }

                return price;
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                browser.Dispose(); // 確保資源被釋放
            }
        }
    }
}
