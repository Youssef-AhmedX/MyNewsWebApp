namespace MyNewsMvc.Core.Dtos
{
    public class NewsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string NewsContent { get; set; } = null!;
        public string CoverImgPath { get; set; } = null!;
        public DateTime PublicationDate { get; set; }
        public int AuthorId { get; set; }
    }
}
