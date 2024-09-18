namespace WebStock.Common.InterFace
{
    public interface IWebCrawler
    {
        Task<decimal> 爬取股價(string stockID);
    }
}
