using Microsoft.AspNetCore.Mvc;
using MyNewsMvc.Models;
using Newtonsoft.Json;
using System.Net;
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
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors").Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var AuthorsList = JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(Data);

            return View(AuthorsList);
        }

        //Create Authors

        [HttpGet]
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

            AuthorDto authorDto = new() { Name = model.Name };

            string Data = JsonConvert.SerializeObject(authorDto);
            StringContent content = new StringContent(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Authors", content).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var AuthorView = JsonConvert.DeserializeObject<AuthorViewModel>(response.Content.ReadAsStringAsync().Result);

            return PartialView("_AuthorNewRow", AuthorView);
        }

        //Edit Authors

        [HttpGet]
        //[AjaxOnly]
        public IActionResult Edit(int Id)
        {
            return PartialView("_Form");
        }


        //Remote Function

        public IActionResult IsExist(AuthorFormViewModel model)
        {

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors/" + model.Name).Result;

            if (!response.IsSuccessStatusCode)
                return Json(false);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var Author = JsonConvert.DeserializeObject<AuthorFormViewModel>(data);

                var IsAllowed = Author is null || Author.Id == model.Id;

                return Json(IsAllowed);
            }

            return Json(true);

        }


    }
}
