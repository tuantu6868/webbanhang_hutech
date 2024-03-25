using System.ComponentModel.DataAnnotations;

namespace NguyenDaiHiep_2180605809_week_three.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
        public List<Product>? Products { get; set; }
    }
}
