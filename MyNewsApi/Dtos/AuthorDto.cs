using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyNewsApi.Dtos
{
    public class AuthorDto 
    {
        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        public string Name { get; set; } = null!;

    }
}
