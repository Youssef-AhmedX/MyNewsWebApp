using Microsoft.AspNetCore.Mvc;
using MyNewsMvc.Core.Consts;
using System.ComponentModel.DataAnnotations;

namespace MyNewsMvc.Core.Models
{
    public class AuthorFormViewModel
    {

        public int Id { get; set; }

        [RegularExpression(RegexPatterns.CharactersOnly_Eng)]
        [StringLength(20, MinimumLength = 3)]
        [Remote("IsExist", "Authors", AdditionalFields = "Id")]
        [Display(Name = "Author Name")]
        public string Name { get; set; } = null!;
    }
}
