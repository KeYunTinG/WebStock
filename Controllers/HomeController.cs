using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebStock.Models;
using WebStock.Service;

namespace WebStock.Controllers
{
    public class HomeController(IStockService _stockService) : Controller
    {
        public async Task <IActionResult> Index()
        {
            var stockTWData = await _stockService.取得所有股票();

            return View(stockTWData);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
