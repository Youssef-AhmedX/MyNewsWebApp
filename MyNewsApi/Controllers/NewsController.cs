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
            var news = await _context.News
                .Include(n => n.Author)
                .Select(n => new { n.Id, n.Title, n.NewsContent, n.CoverImgPath, n.PublicationDate, n.CreationDate, AuthorName = n.Author!.Name })
                .AsNoTracking().ToListAsync();

            return Ok(news);

        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            var news = await _context.News.FindAsync(id);

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

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] NewsDto newsDto)
        {

            var news = await _context.News.FindAsync(id);

            if (news is null)
                return NotFound();

            news.Title = newsDto.Title;
            news.PublicationDate = newsDto.PublicationDate;
            news.NewsContent = newsDto.NewsContent;
            news.CoverImgPath = newsDto.CoverImgPath;


            _context.SaveChanges();

            return Ok(news);

        }

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStatusAsync(int id)
        {

            var news = await _context.News.FindAsync(id);

            if (news is null)
                return NotFound();

            news.IsDeleted = !news.IsDeleted;


            _context.SaveChanges();

            return Ok(news);

        }
    }
}
