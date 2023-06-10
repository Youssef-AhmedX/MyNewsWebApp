namespace MyNewsApi.Dtos
{
    public class UserManagerResponseDto
    {
        public string Message { get; set; } = null!;

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; } = new List<string>();

        public DateTime? ExpireDate { get; set; }    
    }
}
