using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNewsApi.Data;
using MyNewsApi.Dtos;
using MyNewsApi.Models;
using System.Runtime.InteropServices;

namespace MyNewsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AuthorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var authors = await _context.Authors
                .Include(a => a.news)
                .Select(a => new { a.Id,a.Name, NewsCount = a.news.Count() })
                .AsNoTracking().ToListAsync();

            return Ok(authors);

        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByNameAsync(string Name)
        {

            var author = await _context.Authors.SingleOrDefaultAsync(c => c.Name == Name);

            if (author is null)
                return NoContent();

            return Ok(author);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {

            var author = await _context.Authors.FindAsync(id);

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(AuthorDto authorDto)
        {
            Author author = new() { Name = authorDto.Name };

            await _context.AddAsync(author);
            _context.SaveChanges();

            return Ok(author);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id,[FromBody]AuthorDto authorDto)
        {

            var author = await _context.Authors.FindAsync(id);

            if (author is null)
                return NotFound();

            author.Name = authorDto.Name;

            _context.SaveChanges();

            return Ok(author);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author is null)
                return NotFound();

            _context.Remove(author);
            _context.SaveChanges();

            return Ok(author);


        }











    }
}
