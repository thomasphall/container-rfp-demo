using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            var random = new Random(DateTime.Now.Millisecond);
            var id = random.Next(1, 100);
            var queueName = "Subscriber";

            using (var httpClient = new HttpClient())
            {
                // Get the string value
                var value = await httpClient.GetStringAsync($"http://statistics/api/values/{id}");

                // Get the queue depth.
                var httpRequestMessage = new HttpRequestMessage();
                httpRequestMessage.RequestUri = new Uri($"http://statistics/api/QueueDepths/{queueName}");
                var queueDepthResponse = await httpClient.SendAsync(httpRequestMessage);
                var queueDepth = await queueDepthResponse.Content.ReadAsStringAsync();

                ViewData["valueMessage"] = $"Hello from website and {value}";
                ViewData["messageCount"] = queueDepth;
            }

            return View();
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
