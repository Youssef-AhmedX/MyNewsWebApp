namespace MyNewsMvc.Core.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string NewsContent { get; set; } = null!;
        public string CoverImgPath { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public AuthorViewModel Author { get; set; } = null!;
    }
}
