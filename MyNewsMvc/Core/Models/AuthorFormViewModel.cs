using Microsoft.AspNetCore.Mvc;
using MyNewsMvc.Core.Consts;
using System.ComponentModel.DataAnnotations;

namespace MyNewsMvc.Core.Models
{
    public class AuthorFormViewModel
    {

        public int Id { get; set; }

        [RegularExpression(RegexPatterns.CharactersOnly_Eng,ErrorMessage = Errors.EngCharactersOnly)]
        [StringLength(20, MinimumLength = 3,ErrorMessage = Errors.MaxMinLengthError)]
        [Remote("IsExist", "Authors", AdditionalFields = "Id",ErrorMessage =Errors.IsExistError)]
        [Display(Name = "Author Name")]
        public string Name { get; set; } = null!;
    }
}
