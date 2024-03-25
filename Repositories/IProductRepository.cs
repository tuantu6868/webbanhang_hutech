namespace NguyenDaiHiep_2180605809_week_three.Repositories
{
    using System.Collections.Generic;
    using NguyenDaiHiep_2180605809_week_three.Models; // Thay thế bằng namespace thực tế của bạn
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(int id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(int id);

    }
}
