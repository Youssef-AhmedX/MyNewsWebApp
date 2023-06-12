using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MyNewsMvc.Controllers
{
    public class MyNewsController : Controller
    {
        private readonly Uri _baseAddress = new("https://localhost:44354/api");
        private readonly HttpClient _httpClient;

        public MyNewsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = _baseAddress;
        }
        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News").Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var NewsList = JsonConvert.DeserializeObject<IEnumerable<NewsViewModel>>(Data);

            if (NewsList is null)
                return View();

            NewsList = NewsList.Where(n=>n.PublicationDate <= DateTime.Now).OrderByDescending(n => n.PublicationDate).ToList();

            return View(NewsList);
        }
        public IActionResult Details(int id)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetById?id=" + id).Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var News = JsonConvert.DeserializeObject<NewsViewModel>(Data);

            if (News is null)
                return RedirectToAction(nameof(Index));

            return View(News);
        }
    }
}
