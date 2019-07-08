using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Website.Models;

namespace Website.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            var queueName = "Subscriber";

            using (var httpClient = new HttpClient())
            {
                // Get the queue depth.
                ViewData["messageCount"] = await httpClient.GetStringAsync($"http://statistics/api/QueueDepths/{queueName}");
            }

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
