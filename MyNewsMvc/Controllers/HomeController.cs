

using Newtonsoft.Json;

namespace MyNewsMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Uri _baseAddress = new("https://localhost:44354/api");
        private readonly HttpClient _httpClient;

        public HomeController(ILogger<HomeController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
            _httpClient.BaseAddress = _baseAddress;

        }

        public IActionResult Index()
        {
            HttpResponseMessage NewsResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/News").Result;

            if (!NewsResponse.IsSuccessStatusCode)
                return View("_NotFound");

            HttpResponseMessage AuthorsResponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors").Result;

            if (!AuthorsResponse.IsSuccessStatusCode)
                return View("_NotFound");

            string NewsData = NewsResponse.Content.ReadAsStringAsync().Result;
            string AuthorsData = AuthorsResponse.Content.ReadAsStringAsync().Result;

            var NewsList = JsonConvert.DeserializeObject<IEnumerable<NewsViewModel>>(NewsData);
            var AuthorsList = JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(AuthorsData);

            if(NewsList is null || AuthorsList is null)
                return View();

            HomeViewModel HomeView = new()
            {
                AuthorsCount = AuthorsList.Count(),
                NewsCount = NewsList.Where(n => n.PublicationDate <= DateTime.Now).Count(),
            };

            return View(HomeView);
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