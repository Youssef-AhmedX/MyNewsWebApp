using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNewsApi.Data;
using MyNewsApi.Dtos;
using MyNewsApi.Models;

namespace MyNewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var news = await _context.News.Include(n => n.Author).AsNoTracking().ToListAsync();

            return Ok(news);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(NewsDto newsDto)
        {
            News news = new()
            {
                Title = newsDto.Title,
                NewsContent = newsDto.NewsContent,
                CoverImgPath = newsDto.CoverImgPath,
                PublicationDate = newsDto.PublicationDate,
                AuthorId = newsDto.AuthorId,
            };

            news.CreationDate = DateTime.Now;

            await _context.AddAsync(news);
            _context.SaveChanges();

            return Ok(news);

        }
    }
}
