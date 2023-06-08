﻿using Microsoft.AspNetCore.Mvc.Rendering;
using MyNewsMvc.Core.Dtos;
using Newtonsoft.Json;
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

        public IActionResult Index()
        {
            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/News").Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");


            string Data = response.Content.ReadAsStringAsync().Result;
            var NewsList = JsonConvert.DeserializeObject<IEnumerable<NewsViewModel>>(Data);

            return View(NewsList);
        }

        //Create Authors

        [HttpGet]
        public IActionResult Create()
        {
            return View("Form", InitialNewsForm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(NewsFormViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Form", InitialNewsForm(model));


            if (!(model.PublicationDate >= DateTime.Today && model.PublicationDate <= DateTime.Today.AddDays(6)))
            {
                ModelState.AddModelError(nameof(model.PublicationDate), "The date should be between today and a week from today.");
                return View("Form", InitialNewsForm(model));
            }


            var ImgExtension = Path.GetExtension(model.CoverImg.FileName);

            if (!_allowedExtensions.Contains(ImgExtension)) 
            {
                ModelState.AddModelError(nameof(model.PublicationDate), "Only .jpg,.jpeg,.png are allowed!");
                return View("Form", InitialNewsForm(model));
            }

            var ImageName = $"{Guid.NewGuid()}{ImgExtension}" ;
            var ImagePath = Path.Combine($"{_webHostEnvironment.WebRootPath}/Images/NewsCoverImgs", ImageName);

            using var stream = System.IO.File.Create(ImagePath);
            model.CoverImg.CopyTo(stream);


            NewsDto newsDto = new()
            {
                Title = model.Title,
                NewsContent = model.NewsContent,
                PublicationDate = model.PublicationDate,
                AuthorId = model.AuthorId,
                CoverImgPath = ImageName,

            };

            string Data = JsonConvert.SerializeObject(newsDto);
            StringContent content = new(Data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = _httpClient.PostAsync(_httpClient.BaseAddress + "/News", content).Result;

            if (!response.IsSuccessStatusCode)
                return View("_NotFound");

            return RedirectToAction(nameof(Index));
        }

        private NewsFormViewModel InitialNewsForm(NewsFormViewModel? Model = null)
        {

            NewsFormViewModel NewsFormView = Model is null ? new() : Model;

            HttpResponseMessage response = _httpClient.GetAsync(_httpClient.BaseAddress + "/Authors").Result;

            if (!response.IsSuccessStatusCode)
                return NewsFormView;


            string Data = response.Content.ReadAsStringAsync().Result;
            var AuthorsList = JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(Data);

            if(AuthorsList is null)
                return NewsFormView;

            NewsFormView.Authors = AuthorsList.Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() });

            return NewsFormView;

        }

    }
}