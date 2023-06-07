using System.ComponentModel.DataAnnotations;

namespace MyNewsMvc.Models
{
    public class AuthorViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
