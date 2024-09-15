using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebStock.Models;
using WebStock.Models.Interface;
using WebStock.Service;

namespace WebStock.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStockService _stockService;

        public HomeController(ILogger<HomeController> logger,IStockService StockService)
        {
            _logger = logger;
            _stockService = StockService;
        }
        public async Task <IActionResult> Index()
        {
            var stockTWData = await _stockService.GetStock();

            return View(stockTWData);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
