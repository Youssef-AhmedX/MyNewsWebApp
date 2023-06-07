using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace MyNewsMvc.Models
{
    public class AuthorFormViewModel
    {

        public int Id { get; set; }

        [StringLength(20, MinimumLength = 3)]
        [Remote("IsExist", "Authors", AdditionalFields = "Id")]
        [Display(Name = "Author Name")]
        public string Name { get; set; } = null!;
    }
}
