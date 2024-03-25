using Microsoft.EntityFrameworkCore;
using NguyenDaiHiep_2180605809_week_three.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace NguyenDaiHiep_2180605809_week_three.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
    }
}
