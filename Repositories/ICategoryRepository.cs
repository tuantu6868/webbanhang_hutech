namespace NguyenDaiHiep_2180605809_week_three.Repositories
{
    using System.Collections.Generic;
    using NguyenDaiHiep_2180605809_week_three.Models;
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<Category> GetByIdAsync(int id);
        Task AddAsync(Category category);
        Task UpdateAsync(Category category);
        Task DeleteAsync(int id);
    }
}
