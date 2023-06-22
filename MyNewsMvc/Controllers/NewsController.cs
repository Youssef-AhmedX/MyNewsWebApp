using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyNewsMvc.Core.Dtos;
using MyNewsMvc.Core.Models;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.Text;

namespace MyNewsMvc.Controllers
{
    public class NewsController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Uri _baseAddress = new("https://localhost:44354/api");
        private readonly HttpClient _httpClient;

        private readonly List<string> _allowedExtensions = new() { ".jpg", ".jpeg", ".png" };


        public NewsController(HttpClient httpClient, IWebHostEnvironment webHostEnvironment)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = _baseAddress;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetAllPanel").Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var NewsList = JsonConvert.DeserializeObject<IEnumerable<NewsViewModel>>(Data);

            if (NewsList is null)
                return View();

            NewsList = NewsList.OrderByDescending(n => n.PublicationDate).ToList();

            return View(NewsList);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetByIdPanel?id=" + id).Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var News = JsonConvert.DeserializeObject<NewsViewModel>(Data);

            if (News is null)
                return RedirectToAction(nameof(Index));


            return View(News);
        }

        //Create News

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form", InitialNewsForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsFormViewModel model)
        {
            if (model.CoverImg is not null)
            {
                var ImgExtension = Path.GetExtension(model.CoverImg.FileName);

                if (!_allowedExtensions.Contains(ImgExtension))
                {
                    ModelState.AddModelError(nameof(model.PublicationDate), "Only .jpg,.jpeg,.png are allowed!");
                    return View("Form", InitialNewsForm(model));
                }

                if (model.CoverImgPath is not null && model.CoverImg is not null)
                        DeleteImg(model.CoverImgPath);

                var ImageName = AddGetImgPath(ImgExtension, model.CoverImg!);

                model.CoverImgPath = ImageName;
            }

            if (!ModelState.IsValid)
                return View("Form", InitialNewsForm(model));


            if (!(model.PublicationDate >= DateTime.Today && model.PublicationDate <= DateTime.Today.AddDays(6)))
            {
                ModelState.AddModelError(nameof(model.PublicationDate), "The date should be between today and a week from today.");
                return View("Form", InitialNewsForm(model));
            }


            NewsDto newsDto = new()
            {
                Title = model.Title,
                NewsContent = model.NewsContent,
                PublicationDate = model.PublicationDate,
                AuthorId = model.AuthorId,
                CoverImgPath = model.CoverImgPath!,
            };

            string Data = JsonConvert.SerializeObject(newsDto);
            StringContent content = new(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/News", content).Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");

            string ResponseData = response.Content.ReadAsStringAsync().Result;
            var News = JsonConvert.DeserializeObject<NewsViewModel>(ResponseData);

            if (News is null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Details), new { id = News.Id });
        }

        //Edit News

        [HttpGet]
        public IActionResult Edit(int id)
        {

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetById?id=" + id).Result;

            if (!response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.NoContent)
                return BadRequest();

            string data = response.Content.ReadAsStringAsync().Result;
            var Author = JsonConvert.DeserializeObject<NewsFormViewModel>(data);


            return View("Form", InitialNewsForm(Author));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(NewsFormViewModel model)
        {
            if (model.CoverImg is not null)
            {
                var ImgExtension = Path.GetExtension(model.CoverImg.FileName);

                if (!_allowedExtensions.Contains(ImgExtension))
                {
                    ModelState.AddModelError(nameof(model.PublicationDate), "Only .jpg,.jpeg,.png are allowed!");
                    return View("Form", InitialNewsForm(model));
                }

                if (model.CoverImgPath is not null && model.CoverImg is not null)
                    DeleteImg(model.CoverImgPath);

                var ImageName = AddGetImgPath(ImgExtension, model.CoverImg!);

                model.CoverImgPath = ImageName;
            }

            if (!ModelState.IsValid)
                return View("Form", InitialNewsForm(model));

            if (!(model.PublicationDate >= DateTime.Today && model.PublicationDate <= DateTime.Today.AddDays(6)))
            {
                ModelState.AddModelError(nameof(model.PublicationDate), "The date should be between today and a week from today.");
                return View("Form", InitialNewsForm(model));
            }

            NewsDto newsDto = new()
            {
                Title = model.Title,
                NewsContent = model.NewsContent,
                PublicationDate = model.PublicationDate,
                AuthorId = model.AuthorId,
                CoverImgPath = model.CoverImgPath!
            };

            string Data = JsonConvert.SerializeObject(newsDto);
            StringContent content = new(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PutAsync(_httpClient.BaseAddress + "/News/Update?id=" + model.Id, content).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            string data = response.Content.ReadAsStringAsync().Result;
            var news = JsonConvert.DeserializeObject<AuthorViewModel>(data);

            if (news is null)
                return RedirectToAction(nameof(Index));

            return RedirectToAction(nameof(Details), new { id = news.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {

            HttpResponseMessage Getresponse = _httpClient.GetAsync(_httpClient.BaseAddress + "/News/GetById?id=" + id).Result;

            if (!Getresponse.IsSuccessStatusCode || Getresponse.StatusCode == HttpStatusCode.NoContent)
                return BadRequest();

            string Getdata = Getresponse.Content.ReadAsStringAsync().Result;
            var news = JsonConvert.DeserializeObject<NewsViewModel>(Getdata);

            if (news is null)
                return BadRequest();

            HttpResponseMessage response = _httpClient.DeleteAsync(_httpClient.BaseAddress + "/News/" + id).Result;

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            DeleteImg(news.CoverImgPath);

            return Ok();
        }

        //private fuctions

        private NewsFormViewModel InitialNewsForm(NewsFormViewModel? Model = null)
        {

            NewsFormViewModel NewsFormView = Model is null ? new() : Model;

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors").Result;

            if (!response.IsSuccessStatusCode)
                return NewsFormView;


            string Data = response.Content.ReadAsStringAsync().Result;
            var AuthorsList = JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(Data);

            if (AuthorsList is null)
                return NewsFormView;

            NewsFormView.Authors = AuthorsList.OrderBy(a => a.Name).Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });

            return NewsFormView;

        }

        private void DeleteImg (string ImgPath)
        {
            var OldImgPath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/NewsCoverImgs", ImgPath);
            if (System.IO.File.Exists(OldImgPath))
                System.IO.File.Delete(OldImgPath);
        }

        private string AddGetImgPath(string ImgExtension, IFormFile CoverImg)
        {
            var ImageName = $"{Guid.NewGuid()}{ImgExtension}";
            var ImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/NewsCoverImgs", ImageName);

            using var stream = System.IO.File.Create(ImagePath);
            CoverImg.CopyTo(stream);

            return ImageName;
        }

    }
}
