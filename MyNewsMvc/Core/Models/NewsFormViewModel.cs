using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace MyNewsMvc.Core.Models
{
    public class NewsFormViewModel
    {
        public int Id { get; set; }

        [MaxLength(500)]
        //[Remote("IsExist", "News", AdditionalFields = "Id,AuthorId")]
        [Display(Name = "News Title")]
        public string Title { get; set; } = null!;

        [Display(Name = "News Context")]
        public string NewsContent { get; set; } = null!;

        [RequiredIf("Id == 0")]
        [Display(Name = "News Cover Image")]
        public IFormFile? CoverImg { get; set; }


        [Display(Name = "Publication Date")]
        public DateTime PublicationDate { get; set; } = DateTime.Now;

        //[Remote("IsExist", "News", AdditionalFields = "Id,Title")]
        [Display(Name = "News Author")]
        public int AuthorId { get; set; }
        public IEnumerable<SelectListItem>? Authors { get; set; }

        public string? CoverImgPath { get; set; }
    }
}
