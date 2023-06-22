using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class NewsController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var news = await _context.News
                .Include(n => n.Author)
                .Select(n => new { n.Id, n.Title, n.NewsContent, n.CoverImgPath, n.PublicationDate, AuthorName = n.Author!.Name,})
                .AsNoTracking().ToListAsync();

            return Ok(news);

        }

        [HttpGet("GetAllPanel")]
        public async Task<IActionResult> GetAllPanelAsync()
        {
            var news = await _context.News
                .Include(n => n.Author)
                .Select(n => new { n.Id, n.Title, n.NewsContent, n.CoverImgPath, n.PublicationDate, n.CreationDate, AuthorName = n.Author!.Name, })
                .AsNoTracking().ToListAsync();

            return Ok(news);

        }

        [AllowAnonymous]
        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            var news = await _context.News
                .Include(n => n.Author)
                .Select(n => new { n.Id, n.Title, n.NewsContent, n.CoverImgPath, n.PublicationDate, AuthorName = n.Author!.Name, n.AuthorId })
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

            return Ok(news);
        }

        [HttpGet("GetByIdPanel")]
        public async Task<IActionResult> GetByIdPanelAsync(int id)
        {

            var news = await _context.News
                .Include(n => n.Author)
                .Select(n => new { n.Id, n.Title, n.NewsContent, n.CoverImgPath, n.PublicationDate, n.CreationDate, AuthorName = n.Author!.Name, n.AuthorId })
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);

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
            news.AuthorId = newsDto.AuthorId;


            _context.SaveChanges();

            return Ok(news);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news is null)
                return NotFound();

            _context.Remove(news);
            _context.SaveChanges();

            return Ok(news);


        }

    }
}
