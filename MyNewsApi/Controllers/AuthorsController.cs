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
            var authors = await _context.Authors.AsNoTracking().ToListAsync();

            return Ok(authors);

        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateAuthorDto authorDto)
        {
            Author author = new() { Name = authorDto.Name };

            await _context.AddAsync(author);
            _context.SaveChanges();

            return Ok(author);

        }







    }
}
