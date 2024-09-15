namespace WebStock.Common.InterFace
{
    public interface IWebCrawler
    {
        Task<decimal> GetStockPrice(string stockID);
    }
}
