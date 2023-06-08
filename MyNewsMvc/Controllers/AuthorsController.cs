using MyNewsMvc.Core.Dtos;
using Newtonsoft.Json;
using System.Net;
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
            StringContent content = new(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/Authors", content).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var AuthorView = JsonConvert.DeserializeObject<AuthorViewModel>(response.Content.ReadAsStringAsync().Result);

            return PartialView("_AuthorNewRow", AuthorView);
        }

        //Edit Authors

        [HttpGet]
        public IActionResult Edit(int id)
        {

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors/GetById?id=" + id).Result;

            if (!response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent)
                return BadRequest();

            string data = response.Content.ReadAsStringAsync().Result;
            var Author = JsonConvert.DeserializeObject<AuthorFormViewModel>(data);


            return PartialView("_Form", Author);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(AuthorFormViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            AuthorDto authorDto = new() { Name = model.Name, Id = model.Id };

            string Data = JsonConvert.SerializeObject(authorDto);
            StringContent content = new(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/Authors/" + model.Id, content).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            string data = response.Content.ReadAsStringAsync().Result;
            var AuthorView = JsonConvert.DeserializeObject<AuthorViewModel>(data);


            return PartialView("_AuthorNewRow", AuthorView);
        }

        //Delete Authors


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/Authors/" + id).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            string data = response.Content.ReadAsStringAsync().Result;
            var AuthorView = JsonConvert.DeserializeObject<AuthorViewModel>(data);

            return Ok(AuthorView!.Name);
        }

        //Remote Function

        public IActionResult IsExist(AuthorFormViewModel model)
        {

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors/GetByName?Name=" + model.Name).Result;

            if (response.StatusCode == HttpStatusCode.NoContent)
                return Json(true);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                var Author = JsonConvert.DeserializeObject<AuthorFormViewModel>(data);

                var IsAllowed = Author!.Id == model.Id;

                return Json(IsAllowed);
            }

            return Json(false);
        }

    }
}
