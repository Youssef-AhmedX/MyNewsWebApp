using System.ComponentModel.DataAnnotations;

namespace MyNewsApi.Dtos
{
    public class UserLoginDto
    {
        [EmailAddress]
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}
