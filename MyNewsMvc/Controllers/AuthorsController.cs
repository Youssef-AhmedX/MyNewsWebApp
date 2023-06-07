using Microsoft.AspNetCore.Mvc;
using MyNewsMvc.Models;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;

namespace MyNewsMvc.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly Uri _baseAddress = new("https://localhost:44354/api");
        private readonly HttpClient _httpClient;

        public AuthorsController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = _baseAddress;
        }

        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress+"/Authors").Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var AuthorsList = JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(Data);

            return View(AuthorsList);
        }

        [HttpGet]
        //[AjaxOnly]
        public IActionResult Create()
        {
            return PartialView("_Form");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            AuthorDto authorDto = new() { Name =  model.Name };

            string Data = JsonConvert.SerializeObject(authorDto);
            StringContent content = new StringContent(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Authors", content).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var AuthorView = JsonConvert.DeserializeObject<AuthorViewModel>(response.Content.ReadAsStringAsync().Result);

            return PartialView("_AuthorNewRow", AuthorView);
        }


        public IActionResult IsExist(CategoryFormViewModel model)
        {
            var Category = _context.Categories.SingleOrDefault(c => c.Name == model.Name);

            var IsAllowed = Category is null || Category.Id == model.Id;


            return Json(IsAllowed);
        }
    }
}
