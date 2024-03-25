

using Microsoft.AspNetCore.Identity;

namespace NguyenDaiHiep_2180605809_week_three.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
