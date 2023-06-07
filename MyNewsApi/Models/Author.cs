using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyNewsApi.Models
{

    [Index(nameof(Name), IsUnique = true)]
    public class Author 
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string Name { get; set; } = null!;

    }
}
