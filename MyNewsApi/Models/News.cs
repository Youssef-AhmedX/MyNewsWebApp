using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyNewsApi.Models
{
    [Index(nameof(Title),nameof(AuthorId), IsUnique = true)]
    public class News : BaseModel
    {
        public int Id { get; set; }


        [MaxLength(length: 500)]
        public string Title { get; set; } = null!;

        public string NewsContext { get; set; } = null!;

        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;

        public int AuthorId { get; set; }
        public Author? Author { get; set; }

    }
}
