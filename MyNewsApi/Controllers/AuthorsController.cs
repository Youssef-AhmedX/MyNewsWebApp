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
            var authors = await _context.Authors.AsNoTracking().ToListAsync();

            return Ok(authors);

        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var author = await _context.Authors.SingleOrDefaultAsync(c => c.Name == name);

            return Ok(author);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            return Ok(author);
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
