namespace MyNewsMvc.Core.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string NewsContent { get; set; } = null!;
        public string CoverImgPath { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string AuthorName { get; set; } = null!;
    }
}
