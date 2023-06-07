using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace MyNewsApi.Models
{
    [Index(nameof(Email), IsUnique = true)]
    [Index(nameof(UserName), IsUnique = true)]
    public class AppUser : IdentityUser
    {
        [MaxLength(100)]
        public string FullName { get; set; } = null!;


    }
}
